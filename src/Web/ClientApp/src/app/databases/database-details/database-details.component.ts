import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { provideIcons } from '@ng-icons/core';
import { takeUntil } from 'rxjs';
import { ICONS } from 'src/app/common/icon';
import { BackgroundJobDetailsDto, BackgroundJobsClient, DatabaseDetailsDto, DatabasesClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-database-details',
  templateUrl: './database-details.component.html',
  styleUrls: ['./database-details.component.scss'],
  viewProviders: provideIcons(ICONS)
})
export class DatabaseDetailsComponent implements OnInit {
  database: DatabaseDetailsDto;
  jobHistory: BackgroundJobDetailsDto[] | null = null;
  showConnectionString: boolean = false;

  constructor(private route: ActivatedRoute, private client: DatabasesClient, private backgroundJobClient: BackgroundJobsClient) {}

  ngOnInit(): void {
    const dbId = this.route.snapshot.paramMap.get('databaseId');
    if (dbId) {
      this.client.getDatabaseDetails(+dbId)
      .subscribe({
        next: (result) => {
          this.database = result;
        },
        error: (error) => console.error(error),
      });

      this.backgroundJobClient.getBackgroundJobHistory(+dbId, 1,5)
      .subscribe({
        next: (result) => {
          this.jobHistory = result.items;
        },
        error: (error) => console.error(error),
      });
    }
  }

  doShowConnectionString(){
    this.showConnectionString = true;
  }
}
