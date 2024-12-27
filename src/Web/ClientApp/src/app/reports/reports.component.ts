import { Component, OnInit } from '@angular/core';
import { ReportsClient, ReportDto } from '../web-api-client';
import { ToastService } from '../common/toast/toast.service';
import { provideIcons } from '@ng-icons/core';
import { saveAs } from 'file-saver';
import { ICONS } from '../common/icon';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss'],
  viewProviders: provideIcons(ICONS)
})
export class ReportsComponent implements OnInit {
  public reports: ReportDto[] = [];
  public pageNumber: number = 1;
  public pageSize: number = 5;
  public totalPages: number = 0;

  constructor(private reportsClient: ReportsClient, private toastService: ToastService) {}

  ngOnInit(): void {
    this.loadReports();
  }

  loadReports(): void {
    this.reportsClient.getReports(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        this.reports = response.items;
        this.totalPages = response.totalPages;
      },
      error: (error) => {
        this.toastService.showError('Failed to load reports. ' + error.message);
      }
    });
  }

  downloadReport(report: ReportDto): void {
    this.reportsClient.downloadReport(report.id).subscribe({
      next: (response) => {
        const blob = new Blob([response], { type: response.type });
        saveAs(blob, report.fileName);
        this.toastService.showSuccess(`Downloading report: ${report.title}`);
      },
      error: (error) => {
        this.toastService.showError('Failed to download report. ' + error.message);
      }
    });
  }

  deleteReport(report: ReportDto): void {
    this.reportsClient.deleteReport(report.id).subscribe({
      next: () => {
        this.toastService.showSuccess('Report deleted successfully');
        this.loadReports();
      },
      error: (error) => {
        this.toastService.showError('Failed to delete report');
      }
    });
  }

  changePage(event: { pageNumber: number, pageSize: number }): void {
    this.pageNumber = event.pageNumber;
    this.pageSize = event.pageSize;
    this.loadReports(); 
  }

  changePageSize(newPageSize: number): void {
    this.pageSize = newPageSize;
    this.pageNumber = 1;
    this.loadReports(); 
  }
}
