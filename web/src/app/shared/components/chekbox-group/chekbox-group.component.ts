import { Attribute, Component, Input, OnInit, Optional } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'mat-chekbox-group',
  templateUrl: './chekbox-group.component.html',
  styleUrls: ['./chekbox-group.component.scss']
})
export class ChekboxGroupComponent {
  list: { value: any; text: string }[];
  control: FormControl;
  keys: any;
  log: any;
  horizontal: boolean

  constructor(@Optional() @Attribute('horizontal') _horizontal: any) {
    this.horizontal = _horizontal !== undefined && _horizontal !== null ? _horizontal !== "false" : false
  }
  @Input('control') set _control(value: any) {
    this.control = value as FormControl;
  }
  @Input('source') set _source(value: any[]) {
    const type = typeof value[0];

    if (type == 'string' || type == 'number')
      this.list = value.map((x) => ({ value: x, text: '' + x }));
    else {
      this.list = value.map((x) => ({ value: x, text: x['name'] }));
    }


  }
  update(checked: boolean, value: any) {

    const oldValue = this.control.value || [];
    if (!checked) {
      this.control.setValue(
        oldValue.filter((x: any) => x.value != value)
      );
    } else {
      this.control.setValue(
        this.list
          .filter(
            (x: any) => x.value.value == value || oldValue.indexOf(x.value) >= 0
          )
          .map((x) => x.value)
      );
    }



  }
  getChecked(controlValue: any, itemValue: any): boolean {
    if (controlValue) {
      let exist = controlValue.find(m => m.value == itemValue.value);
      if (exist) {
        return true;
      } else {
        return false
      }
    } else {
      return false;
    }

  }
}
