import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FileUploadCaptionComponent } from './file-upload-caption.component';

describe('FileUploadCaptionComponent', () => {
  let component: FileUploadCaptionComponent;
  let fixture: ComponentFixture<FileUploadCaptionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FileUploadCaptionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FileUploadCaptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
