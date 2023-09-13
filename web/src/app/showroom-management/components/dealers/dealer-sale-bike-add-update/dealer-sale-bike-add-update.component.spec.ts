import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DealerSaleBikeAddUpdateComponent } from './dealer-sale-bike-add-update.component';

describe('DealerSaleBikeAddUpdateComponent', () => {
  let component: DealerSaleBikeAddUpdateComponent;
  let fixture: ComponentFixture<DealerSaleBikeAddUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DealerSaleBikeAddUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DealerSaleBikeAddUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
