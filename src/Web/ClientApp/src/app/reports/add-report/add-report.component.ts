import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { ToastService } from 'src/app/common/toast/toast.service';
import { BackgroundJobsClient, CreateReportCommand, DatabasesClient, DatabaseTableDto, DatabaseTablesClient, IdNameDto, ReportFormat, ReportsClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-add-report',
  templateUrl: './add-report.component.html',
  styleUrls: ['./add-report.component.scss'],
})
export class AddReportComponent implements OnInit {
  reportTitle: string = '';
  selectedDatabaseId: number | null = null;
  selectedTableIds: number[] = [];
  selectedFormat: ReportFormat | null = null;
  
  databases: IdNameDto[] = [];
  availableTables: IdNameDto[] = [];
  availableFormats: ReportFormat[] = [ReportFormat.Pdf, ReportFormat.Xlsx, ReportFormat.Html];

  charts: { title: string, table: string, column: string, type: string, availableColumns: string[] }[] = [];
  availableChartTypes: string[] = ['Bar', 'Line', 'Pie'];
  selectedTables: IdNameDto[] = [];

  jobInProgress = new BehaviorSubject<boolean>(false);


  constructor(
    private dbClient: DatabasesClient,
    private reportsClient: ReportsClient, 
    private router: Router,
    private toastService: ToastService,
    private backgroundJobsClient: BackgroundJobsClient
  ) {}

  ngOnInit(): void {
      this.getDatabases();
  }

  getDatabases(){
    this.dbClient.getDatabasesList().subscribe(v => this.databases = v);
  }

  getTables(databaseId: number){
    this.dbClient.getTablesList(databaseId).subscribe(v => this.availableTables = v);
  }

  onSelectedTableIdsChange(): void {
    this.selectedTables = this.availableTables.filter(table => this.selectedTableIds.includes(table.id));
  }

  addChart(): void {
    this.charts.push({ title: '', table: '', column: '', type: '', availableColumns: [] });
  }

  removeChart(index: number): void {
    this.charts.splice(index, 1);
  }

  onTableChange(chart: any, index: number): void {
    const selectedTable = this.selectedTables.find(table => table.id.toString() === chart.table);
    if (selectedTable) {
      this.dbClient.getColumnsList(this.selectedDatabaseId, selectedTable.id, true).subscribe(columns => {
        this.charts[index].availableColumns = columns.map(column => column.name);
      });
    }
  }

  doGenerate() {
    this.jobInProgress.next(true);

    const command = new CreateReportCommand({databaseId: this.selectedDatabaseId, title: this.reportTitle, tableIds:this.selectedTableIds, format: this.selectedFormat});
    this.reportsClient.createReport(command).subscribe({
      next: (jobId) => {
        this.monitorJobStatus(jobId);
      },
      error: (err) => {
        this.toastService.showError('Error creating report: ' + err.message);
      },
    });
  }

  monitorJobStatus(jobId: number): void {
    const jobStatusCheckInterval = 2000;
  
    const checkStatus = () => {
      this.backgroundJobsClient.getBackgroundJobDetails(jobId).subscribe({
        next: (job) => {
          if (job.isCompleted && job.isSucceeded) {

            this.jobInProgress.next(false);
            this.router.navigate(['/reports']);
            this.toastService.showSuccess('Report created successfully.');
          }
          else if (job.isCompleted && !job.isSucceeded) {

            this.jobInProgress.next(false);
            this.toastService.showError('Report failed to create.');
          }
          else {
            setTimeout(checkStatus, jobStatusCheckInterval);
          }
        },
        error: (error) => {
          console.error('Failed to get job status:', error);
          this.jobInProgress.next(false);
        }
      });
    };
  
    checkStatus();
  }
}
