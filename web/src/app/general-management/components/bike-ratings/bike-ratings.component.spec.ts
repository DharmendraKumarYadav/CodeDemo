import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BikeRatingsComponent } from './bike-ratings.component';

describe('BikeRatingsComponent', () => {
  let component: BikeRatingsComponent;
  let fixture: ComponentFixture<BikeRatingsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BikeRatingsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BikeRatingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
