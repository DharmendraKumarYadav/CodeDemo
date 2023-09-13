import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BrokerBikeSalesListViewComponent } from './broker-bike-sales-list-view.component';

describe('BrokerBikeSalesListViewComponent', () => {
  let component: BrokerBikeSalesListViewComponent;
  let fixture: ComponentFixture<BrokerBikeSalesListViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BrokerBikeSalesListViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BrokerBikeSalesListViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
