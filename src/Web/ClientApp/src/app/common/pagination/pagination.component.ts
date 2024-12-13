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
    const pageLimit = 5;
    const halfLimit = Math.floor(pageLimit / 2);

    let startPage = Math.max(1, this.pageNumber - halfLimit);
    let endPage = Math.min(this.totalPages, this.pageNumber + halfLimit);

    if (this.pageNumber - startPage < halfLimit) {
      startPage = Math.max(1, endPage - pageLimit + 1);
    }
    if (endPage - this.pageNumber < halfLimit) {
      endPage = Math.min(this.totalPages, startPage + pageLimit - 1);
    }

    return Array.from({ length: endPage - startPage + 1 }, (_, i) => startPage + i);
  }
}
