import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BikeVariantsComponent } from './bike-variants.component';

describe('BikeVariantsComponent', () => {
  let component: BikeVariantsComponent;
  let fixture: ComponentFixture<BikeVariantsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BikeVariantsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BikeVariantsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
