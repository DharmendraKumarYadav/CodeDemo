import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BikeGeneralDetailComponent } from './bike-general-detail.component';

describe('BikeGeneralDetailComponent', () => {
  let component: BikeGeneralDetailComponent;
  let fixture: ComponentFixture<BikeGeneralDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BikeGeneralDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BikeGeneralDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
