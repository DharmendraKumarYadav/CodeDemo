import { NumericValidator } from 'src/app/shared/validators/common-validator';
import { Attribute, Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BikeSpecificationService } from 'src/app/bike-spec-management/services/bike-specification.service';
import { UserAction } from 'src/app/shared/enums/table-action.enum';
import { ValidationMessage } from 'src/app/shared/message/validation-message';
import { ModalData } from 'src/app/shared/model/common/modal-data.model';
import { NotificationService } from 'src/app/shared/service/notification.service';
import { SpinnerService } from 'src/app/shared/service/spinner.service';
import { UserComponent } from 'src/app/user-management/components/user/user.component';

@Component({
  selector: 'app-budget',
  templateUrl: './budget.component.html',
  styleUrls: ['./budget.component.scss']
})
export class BudgetComponent implements OnInit {
  formGroup: FormGroup;
  isLoading: boolean;
  formName: string = "Create Budget"
  isEdit: boolean = false;
  isAdd: boolean = false;
  isView: boolean = false;
  constructor(@Inject(MAT_DIALOG_DATA) public modalData: ModalData, private spinnerService: SpinnerService, private fb: FormBuilder, public dialogRef: MatDialogRef<BudgetComponent>, private bikeSpecService: BikeSpecificationService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.initializeForm();
    this.checkFormAction(this.modalData);
  }
  checkFormAction(data: ModalData) {
    if (data.action == UserAction.Edit) {
      this.isEdit = true;
      this.formName = "Edit Budget"
      this.populateFormData(data.data);
    } else if (data.action == UserAction.Add) {
      this.formName = "Create Budget"
      this.isAdd = true;
    }
    else if (data.action == UserAction.View) {
      this.formName = "View Budget"

      this.populateFormData(data.data);
      this.isView = true;
      this.formGroup.disable();
    }
  }

  initializeForm() {
    this.formGroup = this.fb.group({
      amount: new FormControl("", [Validators.required,NumericValidator]),
      operator: new FormControl("", [Validators.required]),
      description: new FormControl("", [Validators.required]),
    });
  }
  populateFormData(formData: any) {
    this.formGroup.patchValue(formData);
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
    let spinerRef = this.spinnerService.start();
    this.bikeSpecService.createBudget(formValue).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Budget created sucessfully.");
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
    formValue.id=this.modalData.data.id;
    this.isLoading = true;
    let spinerRef = this.spinnerService.start();
    this.bikeSpecService.updateBudget(formValue,this.modalData.data.id).subscribe(
      (response: any) => {
        this.notificationService.showSuccess
          ("Budget updated sucessfully.");
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
}
