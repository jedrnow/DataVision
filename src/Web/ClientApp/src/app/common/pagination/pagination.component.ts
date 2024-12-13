import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss'],
})
export class PaginationComponent {
  @Input() pageNumber: number = 1;
  @Input() totalPages: number = 1;
  @Input() pageSize: number = 5;
  @Input() pageSizeOptions: number[] = [5, 10, 20, 50];
  @Output() pageChange = new EventEmitter<{ pageNumber: number, pageSize: number }>();
  @Output() pageSizeChange = new EventEmitter<number>();

  changePage(newPage: number): void {
    if (newPage >= 1 && newPage <= this.totalPages) {
      this.pageChange.emit({ pageNumber: newPage, pageSize: this.pageSize });
    }
  }

  changePageSize(newPageSize: number): void {
    this.pageSize = newPageSize;
    this.pageSizeChange.emit(newPageSize);
    this.changePage(1);
  }

  get pages(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }
}
