import { TestBed } from '@angular/core/testing';

import { BikeSpecificationService } from './bike-specification.service';

describe('BikeSpecificationService', () => {
  let service: BikeSpecificationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BikeSpecificationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
