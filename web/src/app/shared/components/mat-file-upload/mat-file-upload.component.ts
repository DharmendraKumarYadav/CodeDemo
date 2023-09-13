import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { interval, of } from 'rxjs';
import { finalize, map } from 'rxjs/operators';
import { Utilities } from '../../service/utilities';
import { FileToUpload } from '../file-upload/models/file-upload.model';
import { FileUploadSource } from '../file-upload/models/upload-source';

@Component({
  selector: 'mat-file-upload',
  templateUrl: './mat-file-upload.component.html',
  styleUrls: ['./mat-file-upload.component.scss']
})
export class MatFileUploadComponent implements OnInit {
  @Output()
  fileChange = new EventEmitter<FileToUpload[]>();
  @Input() allowMultiple: boolean;
  _allowMultiple = true;
  _showCaption = false;
  _throwError = false;
  _showStatusBar = false;
  dataArray = new Array<FileToUpload>();
  constructor() { }

  ngOnInit(): void {
  }
  _uploadSource: FileUploadSource = {

    upload: (file: File, uploadItemId: number) => {
      let progress = 0;
      let allowFileType=['image/jpeg','image/jpg','image/png','application/pdf'];
      return interval(10).pipe(
        finalize(() => {
          let fileObject = Utilities.readUploadFile(file);
          fileObject.uploadItemId = uploadItemId;
          this.dataArray.push(fileObject);
           this.fileChange.emit(this.dataArray);
        }
        ),
        map(() => {
          progress += Math.floor(Math.random() * 10 + 1);

          // if ((uploadItemId === 2 || uploadItemId === 4) && progress > 50) {
          //   throw new Error('Error uploading file');
          // }
          if (!allowFileType.includes(file.type)) {
            throw new Error('File type not supported');
          }
          return Math.min(progress, 100);
        })
      );
    },
    delete: (uploadItemId: number) => {
      this.removeFromArray(uploadItemId);
      this.fileChange.emit(this.dataArray);
      return of(uploadItemId);
    },
  };

  removeFromArray(itemId: number) {
    this.dataArray.forEach((item, index) => {
      if (item.uploadItemId === itemId) this.dataArray.splice(index, 1);
    });
  }
  checkFileHeightWidth(){
    
  }
}
