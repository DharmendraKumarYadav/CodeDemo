import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DealerAvailableBikesComponent } from './dealer-available-bikes.component';

describe('DealerAvailableBikesComponent', () => {
  let component: DealerAvailableBikesComponent;
  let fixture: ComponentFixture<DealerAvailableBikesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DealerAvailableBikesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DealerAvailableBikesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
