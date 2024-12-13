import { Component, OnInit } from '@angular/core';
import { BackgroundJobsClient, DatabaseDto, DatabasesClient } from '../web-api-client';
import { provideIcons } from '@ng-icons/core';
import { ICONS } from '../common/icon';
import { ToastService } from '../common/toast/toast.service';
import { BehaviorSubject, Observable } from 'rxjs';

@Component({
  selector: 'app-databases',
  templateUrl: './databases.component.html',
  styleUrls: ['./databases.component.scss'],
  viewProviders: provideIcons(ICONS)
})
export class DatabasesComponent implements OnInit {
  public databases: DatabaseDto[] = [];
  public pageNumber: number = 1;
  public pageSize: number = 5;
  public totalPages: number = 0;

  public jobStatusMap = new Map<number, BehaviorSubject<boolean>>();

  constructor(private client: DatabasesClient, private backgroundJobsClient: BackgroundJobsClient, private toastService: ToastService) {}

  ngOnInit(): void {
    this.loadDatabases();
  }

  loadDatabases(): void {
    this.client.getDatabases(this.pageNumber, this.pageSize).subscribe({
      next: (result) => {
        this.databases = result.items;
        this.totalPages = result.totalPages;

        this.databases.forEach(db => {
          if (!this.jobStatusMap.has(db.id)) {
            this.jobStatusMap.set(db.id, new BehaviorSubject<boolean>(false));
          }
        });
      },
      error: (error) => console.error(error),
    });
  }

  changePage(event: { pageNumber: number, pageSize: number }): void {
    this.pageNumber = event.pageNumber;
    this.pageSize = event.pageSize;
    this.loadDatabases(); 
  }

  changePageSize(newPageSize: number): void {
    this.pageSize = newPageSize;
    this.pageNumber = 1;
    this.loadDatabases();
  }

  syncDatabase(db: DatabaseDto): void {
    this.jobStatusMap.get(db.id)?.next(true);
  
    this.client.populateDatabase(db.id).subscribe({
      next: (jobId) => {
        this.monitorJobStatus(db.id, jobId);
      },
      error: (error) => {
        console.error(`Failed to sync database ${db.name}:`, error);
      }
    });
  }

  clearDatabase(db: DatabaseDto): void {
    this.jobStatusMap.get(db.id)?.next(true);

    this.client.clearDatabase(db.id).subscribe({
      next: (jobId) => {
        this.monitorJobStatus(db.id, jobId);
      },
      error: (error) => console.error(`Failed to clear database ${db.name}:`, error),
    });
  }

  deleteDatabase(db: DatabaseDto): void {
    this.jobStatusMap.get(db.id)?.next(true);

    this.client.deleteDatabase(db.id).subscribe({
      next: (jobId) => {
        this.monitorJobStatus(db.id, jobId);
      },
      error: (error) => console.error(`Failed to delete database ${db.name}:`, error),
    });
  }

  monitorJobStatus(dbId: number, jobId: number): void {
    const jobStatusCheckInterval = 2000;
  
    const checkStatus = () => {
      this.backgroundJobsClient.getBackgroundJobDetails(jobId).subscribe({
        next: (job) => {
          if (job.isCompleted && job.isSucceeded) {

            this.jobStatusMap.get(dbId)?.next(false);
            this.loadDatabases();

          }
          else if (job.isCompleted && !job.isSucceeded) {

            this.toastService.showError('Sync job failed.');
            this.jobStatusMap.get(dbId)?.next(false);

          }
          else {
            setTimeout(checkStatus, jobStatusCheckInterval);
          }
        },
        error: (error) => {
          console.error('Failed to get job status:', error);
          this.jobStatusMap.get(dbId)?.next(false);
        }
      });
    };
  
    checkStatus();
  }

  isSyncing(db: DatabaseDto): Observable<boolean> {
    return this.jobStatusMap.get(db.id)?.asObservable() || new BehaviorSubject<boolean>(false);
  }

}
