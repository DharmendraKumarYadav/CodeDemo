import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-bike-variants',
  templateUrl: './bike-variants.component.html',
  styleUrls: ['./bike-variants.component.scss']
})
export class BikeVariantsComponent implements OnInit {
  displayedColumns: string[] = [ 'name', 'bookingAmount', 'exShowRoomAmount','insuranceAmount','rtoAmount','totalAmount','select'];
  dataSource : any;
  constructor(@Inject(MAT_DIALOG_DATA) public modalData: any,public dialogRef: MatDialogRef<BikeVariantsComponent>,) { }

  ngOnInit(): void {
    this.dataSource=this.modalData;
  }
  selectedVariant(data:any){
    this.dialogRef.close(data);
  }

}
