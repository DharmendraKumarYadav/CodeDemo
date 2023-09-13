import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ErrorMesageComponent } from './error-mesage.component';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';

describe('ErrorMesageComponent', () => {
  let component: ErrorMesageComponent;
  let fixture: ComponentFixture<ErrorMesageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ErrorMesageComponent ],
      imports:[MatDialogModule],
      providers:[MatDialogRef]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ErrorMesageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
