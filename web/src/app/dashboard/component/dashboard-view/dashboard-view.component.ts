import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { ApplicationModel } from 'src/app/account/model/app.model';
import { map } from 'rxjs/operators';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { DashBoardService } from '../../services/dashboard.service';
import { DashboardModel } from '../../model/dashboard-view.model';
import { SpinnerService } from 'src/app/shared/service/spinner.service';

@Component({
  selector: 'app-dashboard-view',
  templateUrl: './dashboard-view.component.html',
  styleUrls: ['./dashboard-view.component.scss']
})
export class DashboardViewComponent implements OnInit {
  model: DashboardModel;
  miniCard: any[] = new Array<any>();
  isAdminstrator = false;
  /** Based on the screen size, switch from standard to one column per row */
  cardLayout = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map(({ matches }) => {
      if (matches) {
        return {
          columns: 1,
          miniCard: { cols: 1, rows: 1 },
          chart: { cols: 1, rows: 2 },
          table: { cols: 1, rows: 4 },
        };
      }

      return {
        columns: this.isAdminstrator?4:3,
        miniCard: { cols: 1, rows: 1 },
        chart: { cols: this.isAdminstrator?4:3, rows: 2 },
        table: { cols: this.isAdminstrator?4:3, rows: 4 },
      };
    })
  );
  constructor(private authService: AuthenticationService, private breakpointObserver: BreakpointObserver, private spinnerService: SpinnerService, private dashboardService: DashBoardService) { }
  ngOnInit(): void {
    this.getDashboardData();
    this.isAdminstrator = this.authService.isAdminstrator;
  }

  getDashboardData() {
    let ref = this.spinnerService.start();
    this.dashboardService.getDashboardDetails().subscribe(m => {
      this.model = m;
      this.miniCard.push({ title: "Booking Recieved", count: this.model.countModel.bookingCount, text: "Successfully Booked", icon: "date_range", colour: "green" });
      if (this.isAdminstrator) {
        this.miniCard.push({ title: "Total User", count: this.model.countModel.customerCount, text: "All regesterd users", icon: "supervisor_account", colour: "brown" })
      }
      this.miniCard.push({ title: "Ready To Sale", count: this.model.countModel.saleBikeCount, text: "All dealer", icon: "shop_two", colour: "red" })
      this.miniCard.push({ title: "Total Bike", count: this.model.countModel.bikeCount, text: "Total Bike in System", icon: "motorcycle", colour: "blue" })
      this.spinnerService.stop();
    });
  }
}
