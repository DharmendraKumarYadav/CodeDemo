import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DealerSaleBikeComponent } from './dealer-sale-bike.component';

describe('DealerSaleBikeComponent', () => {
  let component: DealerSaleBikeComponent;
  let fixture: ComponentFixture<DealerSaleBikeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DealerSaleBikeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DealerSaleBikeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
