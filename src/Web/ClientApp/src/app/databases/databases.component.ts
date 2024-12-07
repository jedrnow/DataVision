import { Component, OnInit } from '@angular/core';
import { DatabaseDto, DatabasesClient } from '../web-api-client';

@Component({
  selector: 'app-databases',
  templateUrl: './databases.component.html'
})
export class DatabasesComponent implements OnInit {
  public databases: DatabaseDto[] = [];

  constructor(private client: DatabasesClient) {}

  ngOnInit(): void {
    this.client.getDatabases(1,5).subscribe({
      next: result => this.databases = result.items,
      error: error => console.error(error)
  });
  }
}
