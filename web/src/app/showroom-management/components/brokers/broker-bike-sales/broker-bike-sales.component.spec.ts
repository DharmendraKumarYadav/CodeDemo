import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BrokerBikeSalesComponent } from './broker-bike-sales.component';

describe('BrokerBikeSalesComponent', () => {
  let component: BrokerBikeSalesComponent;
  let fixture: ComponentFixture<BrokerBikeSalesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BrokerBikeSalesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BrokerBikeSalesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
