import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BikeFeaturesListComponent } from './bike-features-list.component';

describe('BikeFeaturesListComponent', () => {
  let component: BikeFeaturesListComponent;
  let fixture: ComponentFixture<BikeFeaturesListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BikeFeaturesListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BikeFeaturesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
