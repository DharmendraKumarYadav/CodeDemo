import { Injectable } from '@angular/core';
import { HttpResponseBase, HttpResponse, HttpErrorResponse, HttpEvent, HttpEventType } from '@angular/common/http';
import { tap, filter, map } from 'rxjs/operators';
import { pipe } from 'rxjs';
import { FileToUpload } from '../components/file-upload/models/file-upload.model';
import { DomSanitizer } from '@angular/platform-browser';

@Injectable()
export class Utilities {
  constructor(private _sanitizer: DomSanitizer) {

  }

  public getImagePathFromBase64(base64String: any) {
    return this._sanitizer.bypassSecurityTrustResourceUrl('data:image/jpg;base64,'
      + base64String);
  }

  public static readUploadFile(theFile: any): FileToUpload {
    let file = new FileToUpload();

    // Set File Information
    file.fileName = theFile.name;
    file.fileSize = theFile.size.toString();
    file.fileType = theFile.type;
    // file.lastModifiedTime = theFile.lastModified;    
    // Use FileReader() object to get file to upload
    // NOTE: FileReader only works with newer browsers
    let reader = new FileReader();

    // Setup onload event for reader
    reader.onload = () => {
      // Store base64 encoded representation of file
      file.fileAsBase64 = reader.result.toString();

    }

    // Read the file
    reader.readAsDataURL(theFile);
    return file;
  }

  public static JsonTryParse(value: string) {
    try {
      return JSON.parse(value);
    } catch (e) {
      if (value === 'undefined') {
        return void 0;
      }
      return value;
    }
  }

  public static baseUrl() {
    let base = 'https://cibuat.indusind.com';
    return base;
    if (window.location.origin) {
      base = window.location.origin;
    } else {
      base = window.location.protocol + '//' + window.location.hostname + (window.location.port ? ':' + window.location.port : '');
    }

    return base.replace(/\/$/, '');
  }
  static camelCase(str) {
    return str.replace(/(?:^\w|[A-Z]|\b\w)/g, function (word, index) {
      return index == 0 ? word.toLowerCase() : word.toUpperCase();
    }).replace(/\s+/g, '');
  }
  public static splitWord(str) {
    return str.replace(/([A-Z])/g, ' $1')
      // uppercase the first character
      .replace(/^./, function (str) {
        return str.toUpperCase();
      })
  }


  static toFormData<T>(formValue: T) {
    const formData = new FormData();

    for (const key of Object.keys(formValue)) {
      const value = formValue[key];
      formData.append(key, value);
    }

    return formData;

  }
  static uploadProgress<T>(cb: (progress: number) => void) {
    return tap((event: HttpEvent<T>) => {
      if (event.type === HttpEventType.UploadProgress) {
        cb(Math.round((100 * event.loaded) / event.total));
      }
    });
  }

  static toResponseBody<T>() {
    return pipe(
      filter((event: HttpEvent<T>) => event.type === HttpEventType.Response),
      map((res: HttpResponse<T>) => res.body)
    );
  }
  public static getArrayFromJson(json: any) {
    let array = [];
    for (let obj of json) {
      let a = new Array();
      for (let key in obj) {
        a.push(obj[key])
      }
      array.push(a)
    }
    return array;
  }
  public static validBildeskUrl(url: any, path: string) {
    var parser = document.createElement('a');
    parser.href = url;
    var parts = parser.pathname.split('/');
    var check = parts[1];
    if (path?.toLocaleLowerCase() == check?.toLocaleLowerCase()) {
      return true;
    } else {
      return false;
    }

  }

  public static validBildeskUrlNew(param: any, path: string) {
    param["pid"]
    if (param["pid"] != null && param["data"]) {
      return true;
    } else {
      return false;
    }

  }
  public static validUrl(url: string, baseUrl: string) {
    var parser = document.createElement('a');
    parser.href = url;
    let result = parser.protocol + '//' + parser.hostname
    if (result?.toLocaleLowerCase() == baseUrl) {
      return true;
    } else {
      return false;
    }
  }
}