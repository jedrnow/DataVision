import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CreateReportCommand, DatabasesClient, DatabaseTableDto, DatabaseTablesClient, ReportsClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-add-report',
  templateUrl: './add-report.component.html',
  styleUrls: ['./add-report.component.scss'],
})
export class AddReportComponent implements OnInit {
  reportTitle: string = '';
  selectedDatabaseId: number | null = null;
  selectedTableIds: string[] = [];
  
  databases: DatabaseTableDto[] = [];
  availableTables: DatabaseTableDto[] = [];


  constructor(private dbClient: DatabasesClient, private dbTableClient: DatabaseTablesClient, private reportsClient: ReportsClient) {}

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
    const tableIds = this.selectedTableIds.map(v => +v);
    const command = new CreateReportCommand({databaseId: this.selectedDatabaseId, title: this.reportTitle, tableIds:tableIds});
    this.reportsClient.createReport(command).subscribe(v => console.log(v));
  }
}
