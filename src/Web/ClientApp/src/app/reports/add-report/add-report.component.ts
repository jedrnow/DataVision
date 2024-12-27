import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { ToastService } from 'src/app/common/toast/toast.service';
import { BackgroundJobsClient, CreateReportCommand, DatabasesClient, DatabaseTableDto, DatabaseTablesClient, ReportFormat, ReportsClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-add-report',
  templateUrl: './add-report.component.html',
  styleUrls: ['./add-report.component.scss'],
})
export class AddReportComponent implements OnInit {
  reportTitle: string = '';
  selectedDatabaseId: number | null = null;
  selectedTableIds: string[] = [];
  selectedFormat: ReportFormat | null = null;
  
  databases: DatabaseTableDto[] = [];
  availableTables: DatabaseTableDto[] = [];
  availableFormats: ReportFormat[] = [ReportFormat.Pdf, ReportFormat.Xlsx];

  jobInProgress = new BehaviorSubject<boolean>(false);


  constructor(
    private dbClient: DatabasesClient,
    private dbTableClient: DatabaseTablesClient, 
    private reportsClient: ReportsClient, 
    private router: Router,
    private toastService: ToastService,
    private backgroundJobsClient: BackgroundJobsClient
  ) {}

  ngOnInit(): void {
      this.getDatabases();
  }

  getDatabases(){
    this.dbClient.getDatabases(1,20).subscribe(v => this.databases = v.items);
  }

  getTables(databaseId: number){
    this.dbTableClient.getDatabaseTables(databaseId, 1, 20).subscribe(v => this.availableTables = v.items);
  }

  doGenerate() {
    this.jobInProgress.next(true);

    const tableIds = this.selectedTableIds.map(v => +v);
    const command = new CreateReportCommand({databaseId: this.selectedDatabaseId, title: this.reportTitle, tableIds:tableIds, format: this.selectedFormat});
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
