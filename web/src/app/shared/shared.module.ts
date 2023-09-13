import { NotificationHubService } from './service/notification-hub.service';
import { LOCALE_ID, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutComponent } from './components/layout/layout.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { RouterModule } from '@angular/router';
import { ErrorMesageComponent } from './components/error-mesage/error-mesage.component';
import { AuthLayoutComponent } from './components/auth-layout/auth-layout.component';
import { NoRightClickDirective } from './directive/disable-right-click';
import { BlockCopyPasteDirective } from './directive/copy-paste-prevent';
import { MenuListComponent } from './components/menu-list/menu-list.component';
import { GenericTableComponent } from './components/generic-table/generic-table.component';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTreeModule } from '@angular/material/tree';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatTabsModule } from '@angular/material/tabs';
import { MatChipsModule } from '@angular/material/chips';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatSortModule } from '@angular/material/sort';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatDatepickerModule } from '@angular/material/datepicker'
import { MatBadgeModule } from '@angular/material/badge'
import { MAT_DATE_FORMATS } from '@angular/material/core';
import { MatRadioModule } from '@angular/material/radio';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import { ProgressComponent } from '../shared/components/progress/progress.component';
import { ConfirmDialogComponent } from '../shared/components/confirm-dialog/confirm-dialog.component';
import { AlertDialogComponent } from '../shared/components/alert-dialog/alert-dialog.component';
import { ValidationDialogComponent } from '../shared/components/validation-dialog/validation-dialog.component';
import { DataPropertyGetterPipe } from './pipes/data-property-getter.pipe';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { AccountService } from './service/account.service';
import { AccountEndpoint } from './service/account-endpoint.service';
import { ChekboxGroupComponent } from './components/chekbox-group/chekbox-group.component';
import { FileUploadComponent } from './components/file-upload/file-upload.component';
import { FileUploadCaptionComponent } from './components/file-upload/file-upload-caption/file-upload-caption.component';
import { FileUploadDialogComponent } from './components/file-upload/file-upload-dialog/file-upload-dialog.component';
import { FileUploadItemComponent } from './components/file-upload/file-upload-item/file-upload-item.component';
import { FileUploadStatusBarComponent } from './components/file-upload/file-upload-status-bar/file-upload-status-bar.component';
import { FileUploadZoneComponent } from './components/file-upload/file-upload-zone/file-upload-zone.component';
import { FileUploadInputDirective } from './components/file-upload/directives/file-input.directive';
import { FileUploadItemActionDirective } from './components/file-upload/directives/file-upload-item-action.directive';
import { Utilities } from './service/utilities';
import { MatFileUploadComponent } from './components/mat-file-upload/mat-file-upload.component';
import {  MatStepperModule} from '@angular/material/stepper'
import { ChartsModule } from 'ng2-charts';
import { CommentDialogComponent } from './components/comment-dialog/comment-dialog.component';
import { ViewCommentDialogComponent } from './components/view-comment-dialog/view-comment-dialog.component';
import { NotificationSnackbarComponent } from './components/notification-snackbar/notification-snackbar.component';
import { MultiDropdownComponent } from './components/multi-dropdown/multi-dropdown.component';
import { OverlayModule } from '@angular/cdk/overlay';
import { MatGridListModule } from '@angular/material/grid-list';

export const MY_FORMATS = {
  parse: {
    dateInput: 'DD/MM/YYYY',
  },
  display: {
    dateInput: 'DD/MM/YYYY',
    monthYearLabel: 'MMMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY'
  },
};

@NgModule({
  declarations: [DataPropertyGetterPipe,    FileUploadInputDirective,FileUploadItemActionDirective,
    LayoutComponent, ErrorMesageComponent, AuthLayoutComponent,NoRightClickDirective,BlockCopyPasteDirective, MenuListComponent, GenericTableComponent,ProgressComponent, ConfirmDialogComponent, AlertDialogComponent, ValidationDialogComponent, SpinnerComponent, ChekboxGroupComponent, FileUploadComponent, FileUploadCaptionComponent, FileUploadDialogComponent, FileUploadItemComponent, FileUploadStatusBarComponent, FileUploadZoneComponent, MatFileUploadComponent, CommentDialogComponent, ViewCommentDialogComponent, NotificationSnackbarComponent, MultiDropdownComponent],
  imports: [
    CommonModule,
    RouterModule,
    MatStepperModule,
    FormsModule,
    ReactiveFormsModule,
    MatMomentDateModule,
    MatSidenavModule, MatIconModule, MatToolbarModule, MatButtonModule,
    MatListModule, MatCardModule, MatProgressBarModule, MatInputModule,
    MatSnackBarModule, MatProgressSpinnerModule, MatDatepickerModule,
    MatAutocompleteModule, MatTableModule, MatDialogModule, MatTabsModule, MatRadioModule,
    MatTooltipModule, MatSelectModule, MatPaginatorModule, MatChipsModule,
    MatButtonToggleModule, MatSlideToggleModule, MatBadgeModule, MatCheckboxModule,MatMenuModule,
    MatExpansionModule, DragDropModule, MatSortModule, MatTreeModule, FormsModule, ReactiveFormsModule,
    ChartsModule,
    OverlayModule,
    MatGridListModule
  ],
  providers: [
    {
      provide: MAT_DATE_FORMATS,
      useValue: MY_FORMATS
    },
    { provide: LOCALE_ID, useValue: 'en-gb' },
    AccountService,
    AccountEndpoint,
    NotificationHubService,
    Utilities 
  ],
  exports: [
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    MatMomentDateModule,
    MatSidenavModule, MatIconModule, MatToolbarModule, MatButtonModule,
    MatListModule, MatCardModule, MatProgressBarModule, MatInputModule,
    MatSnackBarModule, MatProgressSpinnerModule, MatDatepickerModule,
    MatAutocompleteModule, MatTableModule, MatDialogModule, MatTabsModule, MatRadioModule,
    MatTooltipModule, MatSelectModule, MatPaginatorModule, MatChipsModule,
    MatButtonToggleModule, MatSlideToggleModule, MatBadgeModule, MatCheckboxModule,
    MatExpansionModule, DragDropModule, MatSortModule, MatTreeModule, FormsModule, ReactiveFormsModule,
    ErrorMesageComponent,DataPropertyGetterPipe,MatMenuModule,
    BlockCopyPasteDirective, MenuListComponent, GenericTableComponent,ProgressComponent, ConfirmDialogComponent, AlertDialogComponent, ValidationDialogComponent,ChekboxGroupComponent,
    FileUploadInputDirective,
    FileUploadComponent,
    FileUploadItemComponent,
    FileUploadCaptionComponent,
    FileUploadItemActionDirective,
    MatFileUploadComponent,
    MatStepperModule,
    ChartsModule,
    SpinnerComponent,
    CommentDialogComponent,
    ViewCommentDialogComponent,
    NotificationSnackbarComponent,
    MultiDropdownComponent,
    MatGridListModule
  ],
  entryComponents: [FileUploadDialogComponent,SpinnerComponent]
})
export class SharedModule { }
