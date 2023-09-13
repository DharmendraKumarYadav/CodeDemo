import { Component, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Label, Color } from 'ng2-charts';
import { DashboardModel } from 'src/app/dashboard/model/dashboard-view.model';
import { DashBoardService } from 'src/app/dashboard/services/dashboard.service';
@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.scss']
})
export class CustomerComponent implements OnInit {

  public lineChartData: ChartDataSets[] = [
    { data: [], label: 'Registered User' },
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
      borderColor: 'brown',
      backgroundColor: 'yellow',
    },
  ];
  public lineChartLegend = true;
  public lineChartType: ChartType = 'bar';
  public lineChartPlugins = [];

  constructor(private dashboardService:DashBoardService) { }

  ngOnInit() {
    this.dashboardService.getDashboardDetails().subscribe({
      next: (coolItems:DashboardModel) => {
        let arrayData=new Array<number>();
        coolItems.monthlyCustomer.forEach(ci => {
          arrayData.push(ci.count);
          this.lineChartLabels.push(ci.monthName);
        });
        this.lineChartData[0].data=arrayData;
    
      },
      error: err =>{}
    });
  }

}
