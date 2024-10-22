/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { UploadService } from './Upload.service';

describe('Service: Upload', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [UploadService]
    });
  });

  it('should ...', inject([UploadService], (service: UploadService) => {
    expect(service).toBeTruthy();
  }));
});
