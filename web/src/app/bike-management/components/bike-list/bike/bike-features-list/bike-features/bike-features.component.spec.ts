import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BikeFeaturesComponent } from './bike-features.component';

describe('BikeFeaturesComponent', () => {
  let component: BikeFeaturesComponent;
  let fixture: ComponentFixture<BikeFeaturesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BikeFeaturesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BikeFeaturesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
