import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DealerSaleBikeListViewComponent } from './dealer-sale-bike-list-view.component';

describe('DealerSaleBikeListViewComponent', () => {
  let component: DealerSaleBikeListViewComponent;
  let fixture: ComponentFixture<DealerSaleBikeListViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DealerSaleBikeListViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DealerSaleBikeListViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
