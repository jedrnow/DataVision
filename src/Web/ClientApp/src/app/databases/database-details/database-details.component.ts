import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { takeUntil } from 'rxjs';
import { DatabaseDetailsDto, DatabasesClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-database-details',
  templateUrl: './database-details.component.html'
})
export class DatabaseDetailsComponent implements OnInit {
  database: DatabaseDetailsDto;
  showConnectionString: boolean = false;

  constructor(private route: ActivatedRoute, private client: DatabasesClient) {}

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
    }
  }

  doShowConnectionString(){
    this.showConnectionString = true;
  }
}
