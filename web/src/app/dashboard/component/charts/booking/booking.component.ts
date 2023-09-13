import { Component, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Color, Label } from 'ng2-charts';
import { DashboardModel } from 'src/app/dashboard/model/dashboard-view.model';
import { DashBoardService } from 'src/app/dashboard/services/dashboard.service';

@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.scss']
})
export class BookingComponent implements OnInit {

  public lineChartData: ChartDataSets[] = [
    { data: [], label: 'No. Of Booking' },
  ];

  public lineChartLabels: Label[] = [];

  public lineChartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    scales: {
      yAxes: [{
          ticks: {
              beginAtZero: true,
              stepSize: 5,
          }
      }]
  }

  };
  public lineChartColors: Color[] = [
    {
      borderColor: 'black',
      backgroundColor: 'green',
    },
  ];
  public lineChartLegend = true;
  public lineChartType: ChartType = 'line';
  public lineChartPlugins = [];

  constructor(private dashboardService:DashBoardService) { }

  ngOnInit() {
    this.dashboardService.getDashboardDetails().subscribe({
      next: (coolItems:DashboardModel) => {
        let arrayData=new Array<number>();
        coolItems.monthlyBooking.forEach(ci => {
          arrayData.push(ci.count);
          this.lineChartLabels.push(ci.monthName);
        });
        this.lineChartData[0].data=arrayData;
    
      },
      error: err => {}
    });
  }

}
