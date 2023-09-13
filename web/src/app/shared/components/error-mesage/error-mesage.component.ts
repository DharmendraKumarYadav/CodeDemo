import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'error-mesage',
  templateUrl: './error-mesage.component.html',
  styleUrls: ['./error-mesage.component.scss']
})
export class ErrorMesageComponent implements OnInit {

  @Input('control') control: any;
  @Input('validation') validation: any;
  constructor() { }

  ngOnInit(): void {
  }

  get errorMessage() {
    let message = '';
    for (let i = 0; this.validation.length > i; i++) {
      let type = this.validation[i].type;
      let error = this.control.hasError(type);
      if (error == true) {
        message = this.validation[i].message;
        break;
      }
    }
    return message;
  }
}

