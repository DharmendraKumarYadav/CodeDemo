export interface NavItem {
  displayName: string;
  disabled?: boolean;
  name?: string;
  iconName: string;
  route?: string;
  visible?: boolean;
  key?: string;
  children?: NavItem[];
}
export const MenuName = {
  DashBoard: 'dashboard',

  UserManagement: 'usermanagement',
  UserManagement_User: 'usermanagement_user',


  ShowRoomManagement: 'showroommanagement',
  ShowRoomManagement_ShowRoom: 'showroommanagement_showroom',
  ShowRoomManagement_Dealers: 'showroommanagement_dealers',
  ShowRoomManagement_Brokers: 'showroommanagement_brokers',
  ShowRoomManagement_AuthorizeRequest: 'showroommanagement_authorizerequest',
  ShowRoomManagement_BikeRequest: 'showroommanagement_bikerequest',
  ShowRoomManagement_DealerSaleBike: 'showroommanagement_dealersalebike',
  ShowRoomManagement_BrokerSaleBike: 'showroommanagement_brokersalebike',

  CatalogManagement: 'catalogmanagement',
  CatalogManagement_Specification: 'catalogmanagement_specification',
  CatalogManagement_Attribute: 'catalogmanagement_attribute',
  CatalogManagement_Category: 'catalogmanagement_category',
  CatalogManagement_Budget: 'catalogmanagement_budget',
  CatalogManagement_Colour: 'catalogmanagement_colour',
  CatalogManagement_BodyStyle: 'catalogmanagement_BodyStyle',
  CatalogManagement_Displacement: 'catalogmanagement_displacement',
  CatalogManagement_City: 'catalogmanagement_city',
  CatalogManagement_Area: 'catalogmanagement_area',


  BikeManagement: 'bikemanagement',
  BikeManagement_CreateBike: 'bikemanagement_createbike',
  BikeManagement_BikeList: 'bikemanagement_bikelist',
  BikeManagement_BikeListCategory: 'bikemanagement_bikelistcategory',

  BookingManagement: 'bookingmanagement',
  BookingManagement_BookingList: 'bikemanagement_bookinglist',

  GeneralManagement: 'generalmanagement',
  GeneralManagement_BikeRating: 'generalmanagement_bikerating',
  GeneralManagement_BikeRequest: 'generalmanagement_bikerequest',

  ReportManagement: 'reportmanagement',
  ReportManagement_BookingReport: 'bikemanagement_bookingreport',
};
export const AdminPermission = [
  'dashboard',

  'usermanagement',
  'usermanagement_user',

  'catalogmanagement',
  'catalogmanagement_specification',
  'catalogmanagement_attribute',
  'catalogmanagement_category',
  'catalogmanagement_budget',
  'catalogmanagement_colour',
  'catalogmanagement_BodyStyle',
  'catalogmanagement_displacement',
  'catalogmanagement_city',
  'catalogmanagement_area',



  'bikemanagement',
  'bikemanagement_createbike',
  'bikemanagement_bikelist',
  'bikemanagement_bikelistcategory',

  'showroommanagement',
  'showroommanagement_showroom',
  'showroommanagement_dealers',
  'showroommanagement_brokers',


  'bookingmanagement',
  'bikemanagement_bookinglist',

  'generalmanagement',
  'generalmanagement_bikerating',
  'generalmanagement_bikerequest',

  'reportmanagement',
  'bikemanagement_bookingreport',
];
export const DealerPermission = [
  'dashboard',
  'showroommanagement',
  'showroommanagement_showroom',
  'showroommanagement_brokers', 'showroommanagement_authorizerequest',
  'showroommanagement_dealersalebike',

  'bookingmanagement',
  'bikemanagement_bookinglist',
  'generalmanagement',
  'generalmanagement_bikerequest',
  'reportmanagement',
  'bikemanagement_bookingreport',
];
export const BrokerPermission = [
  'dashboard',

  'showroommanagement',
  'showroommanagement_showroom',
  'showroommanagement_dealers',
  'showroommanagement_bikerequest',
  'showroommanagement_brokersalebike',

  'bookingmanagement',
  'bikemanagement_bookinglist',
  'generalmanagement',
  'generalmanagement_bikerequest',
  'reportmanagement',
  'bikemanagement_bookingreport',
];

