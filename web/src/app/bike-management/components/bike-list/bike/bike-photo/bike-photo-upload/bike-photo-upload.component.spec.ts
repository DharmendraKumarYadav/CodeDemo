import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BikePhotoUploadComponent } from './bike-photo-upload.component';

describe('BikePhotoUploadComponent', () => {
  let component: BikePhotoUploadComponent;
  let fixture: ComponentFixture<BikePhotoUploadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BikePhotoUploadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BikePhotoUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
