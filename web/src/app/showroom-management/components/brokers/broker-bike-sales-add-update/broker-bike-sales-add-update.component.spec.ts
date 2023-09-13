import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BrokerBikeSalesAddUpdateComponent } from './broker-bike-sales-add-update.component';

describe('BrokerBikeSalesAddUpdateComponent', () => {
  let component: BrokerBikeSalesAddUpdateComponent;
  let fixture: ComponentFixture<BrokerBikeSalesAddUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BrokerBikeSalesAddUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BrokerBikeSalesAddUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
