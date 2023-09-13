import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ExcelService } from '../../service/excel.service';
import { titleCase } from "title-case";

export interface TableColumn {
  name: string; // column name
  dataKey: string; // name of key of the actual data in this column
  isSortable?: boolean; // can a column be sorted?
  isVisible: boolean
}

@Component({
  selector: 'app-generic-table',
  templateUrl: './generic-table.component.html',
  styleUrls: ['./generic-table.component.scss']
})
export class GenericTableComponent implements OnInit, AfterViewInit {
  toPascalCase = (sentence) => sentence
    .toLowerCase()
    .replace(new RegExp(/[-_]+/, 'g'), ' ')
    .replace(new RegExp(/[^\w\s]/, 'g'), '')
    .trim()
    .split(' ')
    .map(word => word[0]
      .toUpperCase()
      .concat(word.slice(1)))
    .join('');
  public tableDataSource = new MatTableDataSource([]);
  public displayedColumns: string[];
  @ViewChild(MatPaginator, { static: false }) matPaginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) matSort: MatSort;

  @Input() isPageable = false;
  @Input() isSortable = false;
  @Input() isFilterable = false;
  @Input() tableColumns: TableColumn[];
  @Input() rowActionIcon: string;
  @Input() tableTitle: string = "List";
  @Input() addBtnText: string = "Add";
  @Input() paginationSizes: number[] = [10, 25, 50, 100];
  @Input() defaultPageSize = this.paginationSizes[0];

  @Input() IsViewButton?= true;
  @Input() IsEditButton?= true;
  @Input() IsDeleteButton?= true;
  @Input() IsAddButton?:boolean= true;
  @Input() IsActionVisible?= true;


  @Output() sort: EventEmitter<Sort> = new EventEmitter();
  @Output() rowAction: EventEmitter<object> = new EventEmitter<object>();

  // this property needs to have a setter, to dynamically get changes from parent component
  @Input() set tableData(data: any[]) {
    this.setTableDataSource(data);
  }

  constructor(public excelService: ExcelService) {

  }

  ngOnInit(): void {
    const columnNames = this.tableColumns.filter(m => m.isVisible == true).map((tableColumn: TableColumn) => tableColumn.name);
    if (this.rowActionIcon) {
      if (this.IsActionVisible) {
        columnNames.push(this.rowActionIcon)
      }
    }
    this.displayedColumns = columnNames;
  }

  // we need this, in order to make pagination work with *ngIf
  ngAfterViewInit(): void {
    this.tableDataSource.paginator = this.matPaginator;
  }

  setTableDataSource(data: any) {
    this.tableDataSource = new MatTableDataSource<any>(data);
    this.tableDataSource.paginator = this.matPaginator;
    this.tableDataSource.sort = this.matSort;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.tableDataSource.filter = filterValue.trim().toLowerCase();
  }

  sortTable(sortParameters: Sort) {
    // defining name of data property, to sort by, instead of column name
    sortParameters.active = this.tableColumns.find(column => column.name === sortParameters.active).dataKey;
    this.sort.emit(sortParameters);
  }

  emitRowAction(row: any, action: any) {
    this.rowAction.emit({ row, action });
  }
  exportToExcel() {
    if (this.tableDataSource.data.length > 0) {
      const fileName = this.tableTitle;
      const title = this.tableTitle + 'Report';
      const header = [];
      const keyArra = this.tableColumns.filter(m => m.isVisible == true).map((tableColumn: TableColumn) => tableColumn.dataKey);
      let data = new Array<any>();
      this.tableDataSource.data.forEach(element => {
        let jsonData = [];
        Object.keys(element).forEach(function (key) {
          if (keyArra.includes(key)) {
            jsonData.push(element[key]);
            // if (!header.includes(pascalCase(titleCase(key)))) {
            //   header.push(pascalCase(titleCase(key)));
            // }

          }
        })
        data.push(jsonData)
      });

      this.excelService.generateExcel(header, data, title, fileName)
    }

  }
}