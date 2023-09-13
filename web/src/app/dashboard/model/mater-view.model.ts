export class MasterViewModel{
    masterModel:MasterModel[];
}
export class MasterModel{

    fileStatus:string;
    zoneRecordModel:ZoneRecordModel[];
}
export class ZoneRecordModel{
    city:string;
    amount:any;
    numberOfFiles:number;
    numberOfRecord:number;
}