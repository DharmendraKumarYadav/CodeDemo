import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RelatedBikeComponent } from './related-bike.component';

describe('RelatedBikeComponent', () => {
  let component: RelatedBikeComponent;
  let fixture: ComponentFixture<RelatedBikeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RelatedBikeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RelatedBikeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
