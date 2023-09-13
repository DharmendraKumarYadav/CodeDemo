import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DealerSaleBikeAuthRequestComponent } from './dealer-sale-bike-auth-request.component';

describe('DealerSaleBikeAuthRequestComponent', () => {
  let component: DealerSaleBikeAuthRequestComponent;
  let fixture: ComponentFixture<DealerSaleBikeAuthRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DealerSaleBikeAuthRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DealerSaleBikeAuthRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
