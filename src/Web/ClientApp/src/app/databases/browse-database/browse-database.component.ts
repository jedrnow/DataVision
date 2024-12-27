import { Component, OnInit } from '@angular/core';
import { 
  DatabaseTableCellDto, 
  DatabaseTableColumnDto, 
  DatabaseTableColumnsClient, 
  DatabaseTableDto, 
  DatabaseTableRowDto, 
  DatabaseTableRowsClient, 
  DatabaseTablesClient 
} from 'src/app/web-api-client';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-browse-database',
  templateUrl: './browse-database.component.html',
})
export class BrowseDatabaseComponent implements OnInit {
  public tables: DatabaseTableDto[] = [];
  public selectedTable: DatabaseTableDto | null = null;
  public columns: DatabaseTableColumnDto[] = [];
  public rows: DatabaseTableRowDto[] = [];
  public selectedTableId: number | null = null;
  public pageNumber: number = 1;
  public pageSize: number = 10;
  public totalRows: number = 0;
  public totalPages: number = 1;

  constructor(
    private tablesClient: DatabaseTablesClient,
    private tableColumnsClient: DatabaseTableColumnsClient, 
    private tableRowsClient: DatabaseTableRowsClient, 
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const dbId = params['databaseId'];
      this.loadTables(dbId);
    });
  }

  loadTables(dbId: string): void {
    this.tablesClient.getDatabaseTables(+dbId, this.pageNumber, this.pageSize).subscribe({
      next: (result) => {
        this.tables = result.items;
        if (this.tables.length > 0) {
          this.selectTable(this.tables[0]);
        }
      },
      error: (error) => console.error('Error loading tables:', error),
    });
  }

  loadColumns(tableId: number): void {
    this.tableColumnsClient.getDatabaseTableColumns(tableId).subscribe({
      next: (columns) => {
        this.columns = columns;
      },
      error: (error) => console.error('Error loading columns:', error),
    });
  }

  loadRows(tableId: number): void {
    this.tableRowsClient.getDatabaseTableRows(tableId, this.pageNumber, this.pageSize).subscribe({
      next: (rows) => {
        this.rows = rows.items;
        this.totalRows = rows.totalCount;
        this.totalPages = rows.totalPages;
      },
      error: (error) => console.error('Error loading rows:', error),
    });
  }

  selectTable(table: DatabaseTableDto): void {
    this.selectedTable = table;
    this.selectedTableId = table.id;
    this.pageNumber = 1;
    this.loadColumns(table.id);
    this.loadRows(table.id);
  }

  getCellValue(row: DatabaseTableRowDto, columnId: number): string {
    const cell = row.cells.find((cell: DatabaseTableCellDto) => cell.databaseTableColumnId === columnId);
    return cell ? cell.value : '';
  }

  changePage(event: { pageNumber: number, pageSize: number }): void {
    this.pageNumber = event.pageNumber;
    this.pageSize = event.pageSize;
    if (this.selectedTableId) {
      this.loadRows(this.selectedTableId);
    }
  }
}
