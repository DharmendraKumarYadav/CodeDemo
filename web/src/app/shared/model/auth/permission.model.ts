export type PermissionNames =
    'Manager User and Roles' | 'Manage Bike' |  'Manage Catalog' | 'Manage Report' | 'Manage Dealer' | 'Manage Booking' ;

export type PermissionValues =
    'user.manage' | 'bike.manage' | 'booking.manage' |
    'catalog.manage' | 'report.manage' | 'dealer.manage';

export class Permission {

    public static readonly manageUserPermission: PermissionValues = 'user.manage';
    public static readonly manageBikePermission: PermissionValues = 'bike.manage';
    public static readonly manageCatalogPermission: PermissionValues = 'catalog.manage';
    public static readonly manageRolesPermission: PermissionValues = 'report.manage';
    public static readonly manageReportPermission: PermissionValues = 'report.manage';
    public static readonly manageDealerPermission: PermissionValues = 'dealer.manage';

    constructor(name?: PermissionNames, value?: PermissionValues, groupName?: string, description?: string) {
        this.name = name;
        this.value = value;
        this.groupName = groupName;
        this.description = description;
    }

    public name: PermissionNames;
    public value: PermissionValues;
    public groupName: string;
    public description: string;
}
