import { Utilities } from './../../../../shared/service/utilities';
import { Component, OnInit, Inject } from "@angular/core";
import { FormGroup, FormBuilder, FormControl, Validators } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BikeSpecificationService } from "src/app/bike-spec-management/services/bike-specification.service";
import { UserAction } from "src/app/shared/enums/table-action.enum";
import { ValidationMessage } from "src/app/shared/message/validation-message";
import { ModalData } from "src/app/shared/model/common/modal-data.model";
import { NotificationService } from "src/app/shared/service/notification.service";
import { SpinnerService } from "src/app/shared/service/spinner.service";

@Component({
  selector: 'app-brand',
  templateUrl: './brand.component.html',
  styleUrls: ['./brand.component.scss']
})
export class BrandComponent implements OnInit {
  formGroup: FormGroup;
  isLoading: boolean;
  formName: string = "Create Brand"
  isEdit: boolean = false;
  isAdd: boolean = false;
  isView: boolean = false;
  _allowMultiple = true;
  _showCaption = false;
  imageUrl:any;
  constructor(@Inject(MAT_DIALOG_DATA) public modalData: ModalData,private utilities:Utilities, private spinnerService: SpinnerService, private fb: FormBuilder, public dialogRef: MatDialogRef<BrandComponent>, private bikeSpecService: BikeSpecificationService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.initializeForm();
    this.checkFormAction(this.modalData);
  }
  checkFormAction(data: ModalData) {
    if (data.action == UserAction.Edit) {
      this.isEdit = true;
      this.formName = "Edit Brand"
      this.populateFormData(data.data);
    } else if (data.action == UserAction.Add) {
      this.formName = "Create Brand"
      this.isAdd = true;
    }
    else if (data.action == UserAction.View) {
      this.formName = "View Brand"

      this.populateFormData(data.data);
      this.isView = true;
      this.formGroup.disable();
    }
  }

  initializeForm() {
    this.formGroup = this.fb.group({
      name: new FormControl("", [Validators.required]),
      description: new FormControl("", [Validators.required]),
      files: new FormControl([],[Validators.required]),
    });
  }
  populateFormData(formData: any) {
    this.formGroup.patchValue(formData);
    this.imageUrl =formData.images!=null? this.utilities.getImagePathFromBase64(formData.images.base64Content):"";
    this.formGroup.get('files').clearValidators();
    this.formGroup.updateValueAndValidity();
  }

  get form() {
    return this.formGroup.controls;
  }

  get validationMessage() {
    return ValidationMessage.catalogForm;
  }
  save() {
    if (this.formGroup.invalid) {
      return;
    }
    let formValue = this.formGroup.value;
    this.isLoading = true;
    this.spinnerService.start();
    this.bikeSpecService.createBrand(formValue).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Brand created sucessfully.");
        this.isLoading = false;
        this.dialogRef.close();
        this.spinnerService.stop();
      },
      (exception: any) => {
        this.isLoading = false;
        this.spinnerService.stop();
        this.notificationService.showValidation(exception);
      }
    );
  }
  update() {
    if (this.formGroup.invalid) {
      return;
    }
    let formValue = this.formGroup.value;
    formValue.id = this.modalData.data.id;
    this.isLoading = true;
    this.spinnerService.start();
    this.bikeSpecService.updateBrand(formValue, this.modalData.data.id).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Brand updated sucessfully.");
        this.isLoading = false;
        this.dialogRef.close();
        this.spinnerService.stop();
      },
      (exception: any) => {
        this.isLoading = false;
        this.spinnerService.stop();
        this.notificationService.showValidation(exception);
      }
    );
  }
  resteForm() {
    this.initializeForm();
  }
  getUploadedFile($event){
    this.formGroup.get("files").setValue($event);
  }
}
