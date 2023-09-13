import { ConfigurationService } from './../../service/configuration.service';
import { AdminPermission, BrokerPermission } from './../../model/common/nav-item';
import { Component, OnInit, OnDestroy, AfterViewInit, ChangeDetectorRef, ViewChild, ElementRef } from '@angular/core';
import { MediaMatcher } from '@angular/cdk/layout';
import { AuthenticationService } from 'src/app/shared/service/auth.service';
import { CdkScrollable } from '@angular/cdk/scrolling';
import { Router } from '@angular/router';
import { User } from 'src/app/shared/model/auth/user.model';
import { NavService } from 'src/app/shared/service/nav.service';
import { DealerPermission, MenuName, NavItem } from 'src/app/shared/model/common/nav-item';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { MatDialog } from '@angular/material/dialog';
import * as signalR from '@microsoft/signalr';  
import { NotificationHubService } from '../../service/notification-hub.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NotificationSnackbarComponent } from '../notification-snackbar/notification-snackbar.component';
import { ConfirmationDialogModel } from '../../model/common/confirm-dialog.model';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit, OnDestroy, AfterViewInit {
  logo = require('../../../../assets/logo/logo.png')
  private _mobileQueryListener: () => void;
  mobileQuery: MediaQueryList;
  showSpinner: boolean;
  userName: string = "Admin User";
  appName: string = "App";
  lastLogin: string = "";
  tickerMessage: any = '';
  isLoggedIn: boolean = false;
  selectedValue: number;
  @ViewChild('snav') appDrawer: ElementRef;
  public user: User;
  public notificationData:any;
  public notificationCount=0;
  
  public userMenuItems: NavItem[] = [];
  navItems: NavItem[] = [
    {
      displayName: 'Dashboard',
      iconName: 'dashboard',
      route: '/dashboard',
      name: MenuName.DashBoard,
      visible: this.isVisible(MenuName.DashBoard)
    },

    {
      displayName: 'User Management',
      iconName: 'supervisor_account',
      name: MenuName.UserManagement,
      visible: this.isVisible(MenuName.UserManagement),
      children: [
        {
          displayName: 'Users',
          iconName: 'account_box',
          route: '/user-management/users',
          name: MenuName.UserManagement_User,
          visible: this.isVisible(MenuName.UserManagement_User)
        },
        {
          displayName: 'Dealer',
          iconName: 'group_work',
          route: '/user-management/dealer',
          name: MenuName.UserManagement_User,
          visible: this.isVisible(MenuName.UserManagement_User)
        },
      ]
    },
    {
      displayName: 'Catalog Management',
      iconName: 'build',
      visible: this.isVisible(MenuName.CatalogManagement),
      children: [
        {
          displayName: 'Specification',
          iconName: 'view_list',
          route: '/bike-spec-management/specification',
          visible: this.isVisible(MenuName.CatalogManagement_Specification),
        },
        {
          displayName: 'Attribute',
          iconName: 'view_list',
          route: '/bike-spec-management/attribute',
          visible: this.isVisible(MenuName.CatalogManagement_Attribute),

        },
        {
          displayName: 'Category',
          iconName: 'view_list',
          route: '/bike-spec-management/category',
          visible: this.isVisible(MenuName.CatalogManagement_Category),

        },
        {
          displayName: 'Brands',
          iconName: 'view_list',
          route: '/bike-spec-management/brands',
          visible: this.isVisible(MenuName.CatalogManagement_Budget)

        },
        {
          displayName: 'Budget',
          iconName: 'view_list',
          route: '/bike-spec-management/budget',
          visible: this.isVisible(MenuName.CatalogManagement_Budget),

        },
        {
          displayName: 'Body Style',
          iconName: 'view_list',
          route: '/bike-spec-management/body-style',
          visible: this.isVisible(MenuName.CatalogManagement_BodyStyle)

        },
        {
          displayName: 'Displacement',
          iconName: 'view_list',
          route: '/bike-spec-management/displacement',
          visible: this.isVisible(MenuName.CatalogManagement_Displacement)

        },
        {
          displayName: 'Colour',
          iconName: 'view_list',
          route: '/bike-spec-management/colour',
          visible: this.isVisible(MenuName.CatalogManagement_Colour)

        },
        {
          displayName: 'City',
          iconName: 'view_list',
          route: '/location/city',
          visible: this.isVisible(MenuName.CatalogManagement_City)
        },
        {
          displayName: 'Area',
          iconName: 'view_list',
          route: '/location/area',
          visible: this.isVisible(MenuName.CatalogManagement_Area)

        },
      ]
    },
    {
      displayName: 'Bike Management',
      iconName: 'motorcycle',
      visible: this.isVisible(MenuName.BikeManagement),
      children: [
        {
          displayName: 'New Bike',
          iconName: 'library_add',
          route: '/bike-management/bike',
          visible: this.isVisible(MenuName.BikeManagement_CreateBike),
        },
        {
          displayName: 'Bike List',
          iconName: 'format_list_bulleted',
          route: '/bike-management/bikes',
          visible: this.isVisible(MenuName.BikeManagement_BikeList)
        },
        {
          displayName: 'Bike Category',
          iconName: 'star',
          route: '/bike-management/bike-features',
          visible: this.isVisible(MenuName.BikeManagement_BikeListCategory)
        }

      ]
    },
    {
      displayName: 'ShowRoom Management',
      iconName: 'store',
      visible: this.isVisible(MenuName.ShowRoomManagement),
      children: [

        {
          displayName: 'Show Rooms',
          iconName: 'home',
          route: '/showroom-management/showromms',
          visible: this.isVisible(MenuName.ShowRoomManagement_ShowRoom)
        },
        {
          displayName: 'Dealers',
          iconName: 'supervisor_account',
          route: '/showroom-management/dealers',
          name: MenuName.ShowRoomManagement_Dealers,
          visible: this.isVisible(MenuName.ShowRoomManagement_Dealers)
        },
        {
          displayName: 'Authorize Requests',
          iconName: 'security',
          route: '/showroom-management/auth-requests',
          visible: this.isVisible(MenuName.ShowRoomManagement_AuthorizeRequest)
        },
        {
          displayName: 'Brokers',
          iconName: 'account_circle',
          route: '/showroom-management/brokers',
          visible: this.isVisible(MenuName.ShowRoomManagement_Brokers)
        },
        {
          displayName: 'Ready To Sale',
          iconName: 'shop',
          route: '/showroom-management/dealer-bikes',
          visible: this.isVisible(MenuName.ShowRoomManagement_DealerSaleBike)
        },

        {
          displayName: 'Ready To Sale',
          iconName: 'shop',
          route: '/showroom-management/bike-sale',
          visible: this.isVisible(MenuName.ShowRoomManagement_BrokerSaleBike)
        },
        {
          displayName: 'Booked Bikes',
          iconName: 'library_books',
          route: '/showroom-management/booked-bikes',
          visible: true
        },
      ]
    },
    // {
    //   displayName: 'Broker Management',
    //   iconName: 'group_work',
    //   visible: this.isVisible(MenuName.BrokerManagement),
    //   children: [
    //     {
    //       displayName: 'Dealers',
    //       iconName: 'account_box',
    //       route: '/broker-management/dealers',
    //       name: MenuName.BrokerManagement_Dealers,
    //       visible: this.isVisible(MenuName.BrokerManagement_Dealers)
    //     },
    //     {
    //       displayName: 'Show Rooms',
    //       iconName: 'view_list',
    //       route: '/broker-management/showromms',
    //       visible: this.isVisible(MenuName.BrokerManagement_ShowRoom)
    //     },
    //     {
    //       displayName: 'Ready To Sale',
    //       iconName: 'view_list',
    //       route: '/broker-management/bike-sale',
    //       visible: this.isVisible(MenuName.BrokerManagement_SaleBike)
    //     }
    //   ]
    // },

 
    {
      displayName: 'Booking Management',
      iconName: 'library_books',
      visible: this.isVisible(MenuName.BookingManagement),
      children: [
        {
          displayName: 'Booking List',
          iconName: 'collections',
          route: '/booking-management/booking',
          visible: this.isVisible(MenuName.BookingManagement_BookingList),
        }
      ]
    },
    {
      displayName: 'General Management',
      iconName: 'featured_play_list',
      visible: this.isVisible(MenuName.GeneralManagement),
      children: [
        {
          displayName: 'Bike Rating',
          iconName: 'stars',
          route: '/general-management/bike-rating',
          visible: this.isVisible(MenuName.GeneralManagement_BikeRating),
        },
        {
          displayName: 'Bike Request',
          iconName: 'call',
          route: '/general-management/bike-request',
          visible: this.isVisible(MenuName.GeneralManagement_BikeRequest),
        }
      ]
    },
    {
      displayName: 'Report Management',
      iconName: 'report',
      visible: this.isVisible(MenuName.ReportManagement),
      children: [
        {
          displayName: 'Booking Report',
          iconName: 'view_list',
          route: '/report-management/booking',
          visible: this.isVisible(MenuName.ReportManagement_BookingReport),
        }
      ]
    },
  ];
  @ViewChild(CdkScrollable) scrollable: CdkScrollable;
  constructor(private changeDetectorRef: ChangeDetectorRef, private router: Router,
    private media: MediaMatcher,private snackBar: MatSnackBar,
    private navService: NavService,
    private notificationHubService:NotificationHubService,
    private authService: AuthenticationService,private configuration:ConfigurationService, public spinnerService: SpinnerService, public dialog: MatDialog) {

    this.mobileQuery = this.media.matchMedia('(max-width: 1000px)');
    this._mobileQueryListener = () => changeDetectorRef.detectChanges();
    this.mobileQuery.addListener(this._mobileQueryListener);
    this.user = this.authService.currentUser;
    if (this.user == null) {
      this.router.navigate(['/auth'])
    } else {
      this.isLoggedIn = true;
    }
  }

  ngOnInit(): void {
    this.userName = this.user.fullName;
     this.getNotifications();  
    const connection = new signalR.HubConnectionBuilder()  
      .configureLogging(signalR.LogLevel.Information)  
      .withUrl(this.configuration.baseUrl+ '/notify')  
      .build();  
  
    connection.start().then(function () {  
      console.log('SignalR Connected!');  
    }).catch(function (err) {  
      return console.error(err.toString());  
    });  
  
    connection.on("BroadcastMessage", () => {
      this.refreshNotiications();  
    }); 

  }
  refreshNotiications(){
    this.notificationHubService.getNotifications().subscribe(m=>{
      this.notificationData=m;
      this.notificationCount=this.notificationData?.count;
        // this.snackBar.openFromComponent(NotificationSnackbarComponent, {
        //   data: 'New notification.',
        //   duration: 12000,
        //   horizontalPosition: "end",
        //   verticalPosition: "top"
        // });
    })
  }
  getNotifications(){
    this.notificationHubService.getNotifications().subscribe(m=>{
      this.notificationData=m;
      // this.notificationCount=this.notificationData?.count;
      console.log("Data Of Notifcation Call");
    })
  }

  ngOnDestroy(): void {
    this.mobileQuery.removeListener(this._mobileQueryListener);
  }
  ngAfterViewInit(): void {
    this.navService.appDrawer = this.appDrawer;
    this.changeDetectorRef.detectChanges();
  }
  logout() {
    this.authService.logout();
  }
  isVisible(menuName): boolean {
    if (this.authService.isDealer) {
      return DealerPermission.includes(menuName)
    } else if (this.authService.isBroker) {
      return BrokerPermission.includes(menuName)
    } else if (this.authService.isAdminstrator) {
      return AdminPermission.includes(menuName)
    }
  }
  clearNotification(){
    this.spinnerService.start();
    this.notificationHubService.clearNotifications().subscribe(()=>{
      this.getNotifications();
      this.spinnerService.stop();
    })
  }
  notificationClick(item:any){
    this.deleteNotification(item.id);
    this.router.navigate([item.url]);
  }

  deleteNotification(id){
    this.notificationHubService.deleteNotifications(id).subscribe(()=>{
      this.getNotifications();
      this.spinnerService.stop();
    })
  }
  onCloseNotification(){
    this.notificationCount=0;
  }
}
