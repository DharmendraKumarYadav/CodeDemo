import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChekboxGroupComponent } from './chekbox-group.component';

describe('ChekboxGroupComponent', () => {
  let component: ChekboxGroupComponent;
  let fixture: ComponentFixture<ChekboxGroupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChekboxGroupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChekboxGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
