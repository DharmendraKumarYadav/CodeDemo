import { filter } from 'rxjs/operators';

import { LocationService } from './../../../../location-management/services/location.service';
import { Utilities } from './../../../../shared/service/utilities';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators, FormArray } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';
import { forkJoin } from 'rxjs';
import { BikeService } from 'src/app/bike-management/services/bike.service';
import { Brand } from 'src/app/bike-spec-management/models/brand.model';
import { Colour } from 'src/app/bike-spec-management/models/colour.model';
import { BikeSpecificationService } from 'src/app/bike-spec-management/services/bike-specification.service';
import { UserAction } from 'src/app/shared/enums/table-action.enum';
import { ValidationMessage } from 'src/app/shared/message/validation-message';
import { Role } from 'src/app/shared/model/auth/role.model';
import { City } from 'src/app/shared/model/common/city.model';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { AccountService } from 'src/app/shared/service/account.service';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { UserComponent } from 'src/app/user-management/components/user/user.component';
import { MatPaginator } from '@angular/material/paginator';
import { BikePhotoUploadComponent } from './bike-photo/bike-photo-upload/bike-photo-upload.component';
import { ConfirmationDialogModel } from 'src/app/shared/model/common/confirm-dialog.model';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { NumericValidator } from 'src/app/shared/validators/common-validator';
import { Dealer } from 'src/app/showroom-management/models/dealer.model';
import { DOCUMENT } from '@angular/common';
import { RelatedBikeComponent } from './related-bike/related-bike.component';

@Component({
  selector: 'app-bike',
  templateUrl: './bike.component.html',
  styleUrls: ['./bike.component.scss']
})
export class BikeComponent implements OnInit {
  isLoading: boolean;
  brands: Brand[];
  colours: Colour[];
  dealers: Dealer[];
  category: any;
  bodyStyle: any;
  cityBike: City[];
  city: City[];
  cityIds: number[];
  specificationList: any[];
  attributeListSpec: any[];
  attributeListFeature: any[];

  bikeBroucher:any;

  bikePhotos: any

  _allowMultiple: any


  isLinear = true;

  generalFormGroup: FormGroup;
  photoFormGroup: FormGroup;
  priceFormGroup: FormGroup;
  specificationFormGroup: FormGroup;
  featureFormGroup: FormGroup;

  bikeId = 0;
  bikePhotoColumns: string[] = ['name', 'imageUrl', 'action'];
  bikePhotoDataSource: any;

  bikeRelatedColumns: string[] = ['name', 'category', 'brandName', 'bodyStyle', 'action'];
  bikeRelatedDataSource: any;

  modalData = new ModalData();

  //Price:
  variantsList: any;



  constructor(@Inject(DOCUMENT) private document: Document, public locationService: LocationService, private dialog: MatDialog, private bikeSpecService: BikeSpecificationService, private utilities: Utilities, private router: Router, private activatedRoute: ActivatedRoute, private bikeService: BikeService, private spinnerService: SpinnerService, private fb: FormBuilder, private accountService: AccountService, private notificationService: NotificationService) {
    this.activatedRoute.params.subscribe(params => {
      let id = params['id'];
      if (id) {
        this.bikeId = id;
      }
    });
  }
  @ViewChild(MatPaginator) paginator: MatPaginator;

  ngAfterViewInit() {
    this.bikePhotoDataSource.paginator = this.paginator;
  }
  ngOnInit(): void {
    this.initializeForm();
    this.loadInitialData();
    this.getSpecification();
    this.getAttributes();
  }

  loadInitialData() {
    let sources = [];
    let brands = this.bikeSpecService.getBrand(-1, -1);
    sources.push(brands);
    let category = this.bikeSpecService.getCategory(-1, -1);
    sources.push(category);
    let bodyStyle = this.bikeSpecService.getBodyStyle(-1, -1);
    sources.push(bodyStyle);
    let colours = this.bikeSpecService.getColour(-1, -1);
    sources.push(colours);

    let generalDetails = this.bikeService.getBikeGenearlDetail(this.bikeId);
    let bikePhotos = this.bikeService.getBikePhotos(this.bikeId);
    let bikePrice = this.bikeService.getBikePrice(this.bikeId);
    let bikeSpecification = this.bikeService.getBikeSpecification(this.bikeId);
    let bikeFeatures = this.bikeService.getBikeFeatures(this.bikeId);
    let bikeSimilar = this.bikeService.getRelatedBikes(this.bikeId);
    let city = this.locationService.getCity(-1, -1);
    sources.push(city);
    if (this.bikeId != 0) {
      sources.push(generalDetails);
      sources.push(bikePhotos);
      sources.push(bikePrice);
      sources.push(bikeSpecification);
      sources.push(bikeFeatures);
      sources.push(bikeSimilar)
    }

    forkJoin(...sources).subscribe((result: any) => {
      this.brands = result[0];
      this.category = result[1];
      this.bodyStyle = result[2];
      this.colours = result[3];
      this.cityBike = result[4];
      if (this.bikeId != 0) {
        this.cityIds = result[5].cityIds;
        console.log("General Detils:", result[5]);
        this.getCity();
        this.addBikeGeneralDetails(result[5]);
        this.populatePhoteData(result[6].images);
        this.populateBikePriceData(result[7])
        this.populateBikeSpecificationData(result[8])
        this.populateBikeFeatureData(result[9])
      } else {
        this.addVariant();
      }
    });

  }


  initializeForm() {
    this.generalFormGroup = this.fb.group({
      id: [0],
      name: new FormControl("", [Validators.required]),
      shortDescription: new FormControl("", [Validators.required]),
      categoryId: new FormControl("", [Validators.required]),
      bodyStyleId: new FormControl("", [Validators.required]),
      longDescription: new FormControl("", [Validators.required]),
      brandId: new FormControl("", [Validators.required]),
      displacement: new FormControl("", [NumericValidator]),
      price: new FormControl("", [Validators.required, NumericValidator]),
      colourIds: new FormControl("", [Validators.required]),
      cityIds: new FormControl("", [Validators.required]),
      isElectricBike:new FormControl("0", [Validators.required]),
      variants: new FormArray([]),
      files: new FormControl(""),
    });

    this.photoFormGroup = this.fb.group({
      bikeId: new FormControl(this.bikeId, [Validators.required]),
      files: new FormControl("", [Validators.required]),
    });
    this.priceFormGroup = this.fb.group({
      bikeVaraintsPrice: this.fb.array([])
    });
    this.specificationFormGroup = this.fb.group({
      bikeVaraintsSpecification: this.fb.array([])
    });
    this.featureFormGroup = this.fb.group({
      bikeVaraintsFeature: this.fb.array([])
    });
  }


  get generalform() {
    return this.generalFormGroup.controls;
  }
  populatePhoteData(bikePhoto: any) {
    bikePhoto = bikePhoto.map(m => {
      m.imageUrl = this.utilities.getImagePathFromBase64(m.images.base64Content);
      m.name = m.images.fileName;
      return m;
    });
    this.bikePhotoDataSource = bikePhoto;
    if (bikePhoto.length > 0) {
      this.photoFormGroup.get('files').clearValidators();
      this.photoFormGroup.get('files').updateValueAndValidity();
    }
  }



  populateBikePriceData(data: any) {
    data.forEach(element => {
      this.addBikeVaraintsPrice(element);
    });

  }

  populateBikeSpecificationData(data: any) {
    data.forEach(element => {
      this.addBikeVaraintsSpecification(element);
    });

  }
  populateBikeFeatureData(data: any) {
    data.forEach(element => {
      this.addBikeVaraintsFeature(element);
    });

  }
  //Bike General Details Populate
  addBikeGeneralDetails(bikes?: any) {
    this.generalFormGroup.patchValue({
      id: this.bikeId,
      name: bikes?.name,
      shortDescription: bikes.shortDescription,
      longDescription: bikes.longDescription,
      brandId: bikes.brandId,
      bodyStyleId: bikes.bodyStyleId,
      categoryId: bikes.categoryId,
      isElectricBike:bikes.isElectricBike,
      colourIds: bikes.colourIds,
      cityIds: bikes.cityIds,
      displacement: bikes?.displacement,
      price: bikes?.price
    });
    if (bikes.variants.length == 0) {
      this.addVariant();
    }
    else {
      bikes.variants.forEach(variant => {
        this.addVariant(variant);
      });
    }

    //Document Available
    if(bikes?.document){
      this.bikeBroucher={
      id:this.bikeId,
      base64 : bikes.document.base64Content,
      name : bikes.document.fileName
      }

    }
  }
  downloadPdf(base64String, fileName) {
    const source = `data:application/pdf;base64,${base64String}`;
    const link = document.createElement("a");
    link.href = source;
    link.download = `${fileName}.pdf`
    link.click();
  }

  //Variant Form Array

  variants(): FormArray {
    return this.generalFormGroup.get("variants") as FormArray
  }
  addVariant(variant?: any) {
    let fg = this.fb.group({
      id: new FormControl(variant?.id != null ? variant?.id : 0),
      name: new FormControl(variant?.name, [Validators.required]),
      specification: new FormControl(variant?.specification, [Validators.required]),
    });
    (<FormArray>this.generalFormGroup.get('variants')).push(fg);
  }
  removeVaraint(i: number) {
    let arrayData = (<FormArray>this.generalFormGroup.controls['variants'])
      .controls[i].value;
    if (arrayData.id) {

      const dialogData = new ConfirmationDialogModel('Confirm', 'Are you sure you want to delete? ');
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        maxWidth: '400px',
        closeOnNavigation: true,
        data: dialogData
      })

      dialogRef.afterClosed().subscribe(dialogResult => {
        if (dialogResult) {
          let ref = this.spinnerService.start();
          this.bikeService.deleteBikeVariants(arrayData.id).subscribe(result => {
            this.spinnerService.stop();
            this.isLoading = false;
            this.notificationService.showSuccess
              ("Variants deleted sucessfully.");
            this.variants().removeAt(i);
            this.document.location.reload();
          });
        } else {

        }
      });



    } else {
      this.variants().removeAt(i);
    }

  }

  //Price Form Array
  addBikeVaraintsPrice(bikeVarints?: any) {
    let fg = this.fb.group({
      variantName: new FormControl({ value: bikeVarints.name, disabled: true }),
      variantId: new FormControl(bikeVarints.variantId, [Validators.required]),
      prices: this.fb.array([])
    });
    (<FormArray>this.priceFormGroup.get('bikeVaraintsPrice')).push(fg);
    let bikeVariantIndex = (<FormArray>this.priceFormGroup.get('bikeVaraintsPrice')).length - 1;
    if (bikeVarints.prices.length == 0) {
      this.addPrice(bikeVariantIndex);
    }
    else {
      bikeVarints.prices.forEach(price => {
        this.addPrice(bikeVariantIndex, price);
      });
    }
  }

  bikeVaraintsPrice(): FormArray {
    return this.priceFormGroup.get('bikeVaraintsPrice') as FormArray;
  }

  prices(bikeVaraintIndex: number): FormArray {
    return this.bikeVaraintsPrice()
      .at(bikeVaraintIndex)
      .get('prices') as FormArray;
  }
  addPrice(bikeVaraintIndex: number, data?: any) {
    let fg = this.fb.group({
      id: new FormControl(data?.id == undefined ? 0 : data?.id),
      isMinPrice: new FormControl(data?.isMinPrice),
      bookingAmount: new FormControl(data?.bookingAmount, [Validators.required, NumericValidator]),
      rtoAmount: new FormControl(data?.rtoAmount, [Validators.required, NumericValidator]),
      insuranceAmount: new FormControl(data?.insuranceAmount, [Validators.required, NumericValidator]),
      exShowRoomAmount: new FormControl(data?.exShowRoomAmount, [Validators.required, NumericValidator]),
      totalAmount: new FormControl({ value: data?.totalAmount, disabled: true }),
      cityId: new FormControl(data?.cityId, [Validators.required]),
    });
    (<FormArray>(<FormGroup>(<FormArray>this.priceFormGroup.controls['bikeVaraintsPrice'])
      .controls[bikeVaraintIndex]).controls['prices']).push(fg);

  }
  calculateValues(bikeVaraintIndex: number, priceIndex: number) {
    let arrayData = (<FormArray>(<FormGroup>(<FormArray>this.priceFormGroup.controls['bikeVaraintsPrice'])
      .controls[bikeVaraintIndex]).controls['prices']).controls[priceIndex].value;

    (<FormArray>(<FormGroup>(<FormArray>this.priceFormGroup.controls['bikeVaraintsPrice'])
      .controls[bikeVaraintIndex]).controls['prices']).controls[priceIndex].patchValue({
        totalAmount: ((+arrayData.exShowRoomAmount + (+arrayData.insuranceAmount) + (+arrayData.rtoAmount)).toString())
      });
  }
  removePrice(bikeVaraintIndex: number, priceIndex: number) {
    let arrayData = (<FormArray>(<FormGroup>(<FormArray>this.priceFormGroup.controls['bikeVaraintsPrice'])
      .controls[bikeVaraintIndex]).controls['prices']).controls[priceIndex].value
    if (arrayData.id) {

      const dialogData = new ConfirmationDialogModel('Confirm', 'Are you sure you want to delete? ');
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        maxWidth: '400px',
        closeOnNavigation: true,
        data: dialogData
      })

      dialogRef.afterClosed().subscribe(dialogResult => {
        if (dialogResult) {
          let ref = this.spinnerService.start();
          this.bikeService.deleteBikePrice(arrayData.id).subscribe(result => {
            this.spinnerService.stop();
            this.isLoading = false;
            this.notificationService.showSuccess
              ("Price deleted sucessfully.");
            this.prices(bikeVaraintIndex).removeAt(priceIndex);
          });
        } else {

        }
      });



    } else {
      this.prices(bikeVaraintIndex).removeAt(priceIndex);
    }

  }

  //Specfication:


  addBikeVaraintsSpecification(bikeVarints?: any) {
    let fg = this.fb.group({
      variantName: new FormControl({ value: bikeVarints.name, disabled: true }),
      variantId: new FormControl(bikeVarints.variantId, [Validators.required]),
      specifications: this.fb.array([])
    });
    (<FormArray>this.specificationFormGroup.get('bikeVaraintsSpecification')).push(fg);
    let bikeVariantIndex = (<FormArray>this.specificationFormGroup.get('bikeVaraintsSpecification')).length - 1;
    if (!bikeVarints) {
      this.addSpecification(bikeVariantIndex);
    }
    else {
      bikeVarints.specifications.forEach(specification => {
        this.addSpecification(bikeVariantIndex, specification);
      });
    }
  }
  bikeVaraintsSpecification(): FormArray {
    return this.specificationFormGroup.get('bikeVaraintsSpecification') as FormArray;
  }

  specifications(bikeVaraintIndex: number): FormArray {
    return this.bikeVaraintsSpecification()
      .at(bikeVaraintIndex)
      .get('specifications') as FormArray;
  }
  addSpecification(bikeVaraintIndex: number, data?: any) {
    let fg = this.fb.group({
      id: new FormControl(data?.id == undefined ? 0 : data?.id),
      specificationId: new FormControl(data?.specificationId, [Validators.required]),
      attributeId: new FormControl(data?.attributeId, [Validators.required]),
      attributeValue: new FormControl(data?.attributeValue, [Validators.required])
    });
    (<FormArray>(<FormGroup>(<FormArray>this.specificationFormGroup.controls['bikeVaraintsSpecification'])
      .controls[bikeVaraintIndex]).controls['specifications']).push(fg);

  }
  removeSpecification(bikeVaraintIndex: number, specIndex: number) {
    let arrayData = (<FormArray>(<FormGroup>(<FormArray>this.specificationFormGroup.controls['bikeVaraintsSpecification'])
      .controls[bikeVaraintIndex]).controls['specifications']).controls[specIndex].value
    if (arrayData.id) {

      const dialogData = new ConfirmationDialogModel('Confirm', 'Are you sure you want to delete? ');
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        maxWidth: '400px',
        closeOnNavigation: true,
        data: dialogData
      })

      dialogRef.afterClosed().subscribe(dialogResult => {
        if (dialogResult) {
          let ref = this.spinnerService.start();
          this.bikeService.deleteBikeSpecification(arrayData.id).subscribe(result => {
            this.spinnerService.stop();
            this.isLoading = false;
            this.notificationService.showSuccess
              ("Specification deleted sucessfully.");
            this.specifications(bikeVaraintIndex).removeAt(specIndex);
          });
        } else {

        }
      });
    } else {
      this.specifications(bikeVaraintIndex).removeAt(specIndex);
    }
  }


  //Bike Features:


  addBikeVaraintsFeature(bikeVarints?: any) {
    let fg = this.fb.group({
      variantName: new FormControl({ value: bikeVarints.name, disabled: true }),
      variantId: new FormControl(bikeVarints.variantId, [Validators.required]),
      features: this.fb.array([])
    });
    (<FormArray>this.featureFormGroup.get('bikeVaraintsFeature')).push(fg);
    let bikeVariantIndex = (<FormArray>this.featureFormGroup.get('bikeVaraintsFeature')).length - 1;

    if (bikeVarints.features.length == 0) {
      this.addFeature(bikeVariantIndex);
    }
    else {
      bikeVarints.features.forEach(feature => {
        this.addFeature(bikeVariantIndex, feature);
      });
    }
  }
  bikeVaraintsFeature(): FormArray {
    return this.featureFormGroup.get('bikeVaraintsFeature') as FormArray;
  }

  features(bikeVaraintIndex: number): FormArray {
    return this.bikeVaraintsFeature()
      .at(bikeVaraintIndex)
      .get('features') as FormArray;
  }
  addFeature(bikeVaraintIndex: number, data?: any) {
    let fg = this.fb.group({
      id: new FormControl(data?.id == undefined ? 0 : data?.id),
      attributeId: new FormControl(data?.attributeId, [Validators.required]),
      attributeValue: new FormControl(data?.attributeValue, [Validators.required])
    });
    (<FormArray>(<FormGroup>(<FormArray>this.featureFormGroup.controls['bikeVaraintsFeature'])
      .controls[bikeVaraintIndex]).controls['features']).push(fg);

  }
  removeFeatures(bikeVaraintIndex: number, featureIndex: number) {
    let arrayData = (<FormArray>(<FormGroup>(<FormArray>this.featureFormGroup.controls['bikeVaraintsFeature'])
      .controls[bikeVaraintIndex]).controls['features']).controls[featureIndex].value
    if (arrayData.id) {

      const dialogData = new ConfirmationDialogModel('Confirm', 'Are you sure you want to delete? ');
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        maxWidth: '400px',
        closeOnNavigation: true,
        data: dialogData
      })

      dialogRef.afterClosed().subscribe(dialogResult => {
        if (dialogResult) {
          let ref = this.spinnerService.start();
          this.bikeService.deleteBikeFeatures(arrayData.id).subscribe(result => {
            this.spinnerService.stop();
            this.isLoading = false;
            this.notificationService.showSuccess
              ("Features deleted sucessfully.");
            this.features(bikeVaraintIndex).removeAt(featureIndex);
          });
        } else {

        }
      });
    } else {
      this.features(bikeVaraintIndex).removeAt(featureIndex);
    }


  }

  get validationMessage() {
    return ValidationMessage.bikeCreateForm;
  }

  //Save Data to Server

  saveGeneralDetails() {
    if (this.generalFormGroup.invalid) {
      return;
    }
    let formValue = this.generalFormGroup.value;
    this.isLoading = true;
    this.spinnerService.start();
    this.bikeService.saveBikeGenearlDetail(formValue).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Details Saved Sucessfully.");
        this.isLoading = false;
        this.spinnerService.stop();
        this.bikeId == 0 ? this.router.navigate(['/bike-management/bike', response]) : this.document.location.reload();
      },
      (exception: any) => {
        this.isLoading = false;
        this.spinnerService.stop();
        this.notificationService.showValidation(exception);
      }
    );
  }


  create() {
    this.modalData.action = "ADD";
    this.modalData.data = this.bikeId;
    const dialogRef = this.dialog.open(BikePhotoUploadComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getBikePhotos(this.bikeId);
    });
  }
  addRelatedBike() {
    this.modalData.action = "ADD";
    this.modalData.data = this.bikeId;
    const dialogRef = this.dialog.open(RelatedBikeComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getRealtedBikes(this.bikeId);
    });
  }
  viewPhoto(rowData: any) {
    this.modalData.action = "VIEW";
    this.modalData.data = rowData.imageUrl;
    const dialogRef = this.dialog.open(BikePhotoUploadComponent, {
      data: this.modalData
    });
    dialogRef.afterClosed().subscribe(result => {
    });
  }
  getUploadedFile($event) {
    this.photoFormGroup.get("files").setValue($event);
  }

  savePhotoDetails() {
    if (this.generalFormGroup.invalid) {
      return;
    }
    let formValue = this.photoFormGroup.value;
    formValue.bikeId = this.bikeId;
    this.isLoading = true;
    let spinerRef = this.spinnerService.start();
    this.bikeService.saveBikePhoto(formValue).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("User created sucessfully.");
        this.isLoading = false;
        this.spinnerService.stop();
      },
      (exception: any) => {
        this.isLoading = false;
        this.spinnerService.stop();
        this.notificationService.showValidation(exception);
      }
    );
  }
  removePhoto(bikePhotoId) {
    const dialogData = new ConfirmationDialogModel('Confirm', 'Are you sure you want to delete? ');
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      closeOnNavigation: true,
      data: dialogData
    })

    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult) {
        let ref = this.spinnerService.start();
        this.bikeService.deleteBikePhoto(bikePhotoId).subscribe(result => {
          this.getBikePhotos(this.bikeId);
          this.spinnerService.stop();
          this.isLoading = false;
          this.notificationService.showSuccess
            ("Photo deleted sucessfully.");
        });
      } else {

      }
    });
  }

  deleteSimilarBike(id) {
    const dialogData = new ConfirmationDialogModel('Confirm', 'Are you sure you want to delete? ');
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      closeOnNavigation: true,
      data: dialogData
    })

    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult) {
        let ref = this.spinnerService.start();
        this.bikeService.deleteBikeRelated(id).subscribe(result => {
          this.getRealtedBikes(this.bikeId);
          this.spinnerService.stop();
          this.isLoading = false;
          this.notificationService.showSuccess
            ("Similar Bike deleted sucessfully.");
        });
      } else {

      }
    });
  }
  //Price Details
  getPriceDetails(bikeId: number) {
    this.isLoading = true;
    this.bikeService.getBikePhotos(bikeId)
      .subscribe(results => {
        this.populatePhoteData(results.images);
        this.isLoading = false
      });

  }
  savePriceDetails() {

    if (this.priceFormGroup.invalid) {
      return;
    }
    let priceDetails = this.priceFormGroup.getRawValue();
    if (priceDetails.bikeVaraintsPrice[0].prices.length == 0) {
      this.notificationService.showError("Please add atleast one record.");
      return;
    }

    let formValue = this.priceFormGroup.getRawValue();
    this.isLoading = true;
    let spinerRef = this.spinnerService.start();
    this.bikeService.saveBikePrice(formValue.bikeVaraintsPrice).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Price detail saved sucessfully.");
        this.isLoading = false;
        this.initializeForm();
        this.loadInitialData();
        this.spinnerService.stop();
      },
      (exception: any) => {
        this.isLoading = false;
        this.spinnerService.stop();
        this.notificationService.showValidation(exception);
      }
    );
  }

  //Specifcation Details
  geBikeSepecifications(bikeId: number) {
    this.isLoading = true;
    this.bikeService.getBikePhotos(bikeId)
      .subscribe(results => {
        this.populatePhoteData(results.images);
        this.isLoading = false
      });
  }
  saveSpecificationDetails() {
    if (this.specificationFormGroup.invalid) {
      return;
    }


    let specificationDetails = this.specificationFormGroup.getRawValue();
    if (specificationDetails.bikeVaraintsSpecification[0].specifications.length == 0) {
      this.notificationService.showError("Please add atleast one record.");
      return;
    }

    let formValue = this.specificationFormGroup.value;
    this.isLoading = true;
    let spinerRef = this.spinnerService.start();
    this.bikeService.saveBikeSpecification(formValue.bikeVaraintsSpecification
    ).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Specifcation detail saved sucessfully.");
        this.isLoading = false;
        this.initializeForm();
        this.loadInitialData();
        this.spinnerService.stop();
      },
      (exception: any) => {
        this.isLoading = false;
        this.spinnerService.stop();
        this.notificationService.showValidation(exception);
      }
    );


  }

  //Feature Details
  geBikeFeatures(bikeId: number) {
    this.isLoading = true;
    this.bikeService.getBikePhotos(bikeId)
      .subscribe(results => {
        this.populatePhoteData(results.images);
        this.isLoading = false
      });
  }
  saveFeatureDetails() {
    if (this.featureFormGroup.invalid) {
      return;
    }
    let featureDetails = this.featureFormGroup.getRawValue();
    if (featureDetails.bikeVaraintsFeature
    [0].features.length == 0) {
      this.notificationService.showError("Please add atleast one record.");
      return;
    }
    let formValue = this.featureFormGroup.value;
    this.isLoading = true;
    let spinerRef = this.spinnerService.start();
    this.bikeService.saveBikeFeatures(formValue.bikeVaraintsFeature).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Features detail saved sucessfully.");
        this.isLoading = false;
        this.initializeForm();
        this.loadInitialData();
        this.spinnerService.stop();
      },
      (exception: any) => {
        this.isLoading = false;
        this.spinnerService.stop();
        this.notificationService.showValidation(exception);
      }
    );

  }

  getSpecification() {
    this.isLoading = true;
    this.bikeSpecService.getSpecification(-1, -1)
      .subscribe(results => {
        this.specificationList = results;
        this.isLoading = false
      });
  }
  getAttributes() {
    this.isLoading = true;
    this.bikeSpecService.getAttribute(-1, -1)
      .subscribe(results => {
        this.attributeListSpec = results.filter(m=>m.type==1);
        this.attributeListFeature = results.filter(m=>m.type==2);
        this.isLoading = false
      });
  }
  getCity() {
    this.isLoading = true;
    this.locationService.getCity(-1, -1)
      .subscribe(results => {
        this.city=new Array<City>();
        console.log("Test",this.cityIds);
        results.forEach(element => {
          if(this.cityIds?.includes(element.id)){
            this.city.push({
              id:element.id,
              name:element.name
            });
          } 
         
        });
        this.isLoading = false
      });
  }

  getBikePhotos(bikeId: number) {
    this.isLoading = true;
    this.bikeService.getBikePhotos(bikeId)
      .subscribe(results => {
        this.populatePhoteData(results.images);
        this.isLoading = false
      });
  }
  getRealtedBikes(bikeId: number) {
    this.isLoading = true;
    this.bikeService.getRelatedBikes(bikeId)
      .subscribe(results => {
        this.populateSimilarData(results);
        this.isLoading = false
      });
  }
  populateSimilarData(similarBike: any) {

    this.bikeRelatedDataSource = similarBike;
  }

  getUploadedBrpucher($event) {
    this.generalFormGroup.get("files").setValue($event);
  }
  removeBroucer(id){
    const dialogData = new ConfirmationDialogModel('Confirm', 'Are you sure you want to delete? ');
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      closeOnNavigation: true,
      data: dialogData
    })

    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult) {
        let ref = this.spinnerService.start();
        this.bikeService.deleteBikeBroucher(id).subscribe(result => {
          this.spinnerService.stop();
          this.isLoading = false;
          this.notificationService.showSuccess
            ("Document deleted sucessfully.");
          this.document.location.reload();
        });
      } else {

      }
    });

  }
}
