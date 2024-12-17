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
  isModalOpen: boolean = false;
  modalContent: string = '';

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


  openModal(jsonString: string): void {
    try {
      const parsedObject = JSON.parse(jsonString);
      this.modalContent = JSON.stringify(parsedObject, null, 2); 
      this.isModalOpen = true;
    } catch (error) {
      this.modalContent = jsonString;
      this.isModalOpen = true;
    }
  }

  openModalWithConnectionString(connectionString: string): void {
    const formattedContent = connectionString.split(';').join(';\n');
    this.modalContent = formattedContent;
    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
  }

  doShowConnectionString(){
    this.showConnectionString = true;
  }
}
