import { Component, OnInit } from '@angular/core';
import { DatabaseDto, DatabasesClient } from '../web-api-client';
import { provideIcons } from '@ng-icons/core';
import { ICONS } from '../common/icon';

@Component({
  selector: 'app-databases',
  templateUrl: './databases.component.html',
  viewProviders: provideIcons(ICONS)
})
export class DatabasesComponent implements OnInit {
  public databases: DatabaseDto[] = [];
  public pageNumber: number = 1;
  public pageSize: number = 5;
  public totalPages: number = 0;
  public disableAll: boolean = false;

  constructor(private client: DatabasesClient) {}

  ngOnInit(): void {
    this.loadDatabases();
  }

  loadDatabases(): void {
    this.client.getDatabases(this.pageNumber, this.pageSize).subscribe({
      next: (result) => {
        this.databases = result.items;
        this.totalPages = result.totalPages;
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
    this.disableAll = true;

    this.client.populateDatabase(db.id).subscribe({
      next: () => {
        this.disableAll = false;
        db.isPopulated = true;
      },
      error: (error) => console.error(`Failed to sync database ${db.name}:`, error),
    });
  }

  clearDatabase(db: DatabaseDto): void {
    this.disableAll = true;

    this.client.clearDatabase(db.id).subscribe({
      next: () => {
        this.disableAll = false;
        db.isPopulated = false;
      },
      error: (error) => console.error(`Failed to clear database ${db.name}:`, error),
    });
  }

  deleteDatabase(db: DatabaseDto): void {
    this.disableAll = true;

    this.client.deleteDatabase(db.id).subscribe({
      next: () => {
        this.disableAll = false;
        this.loadDatabases();
      },
      error: (error) => console.error(`Failed to delete database ${db.name}:`, error),
    });
  }
}
