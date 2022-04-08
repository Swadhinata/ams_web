import { TestBed } from '@angular/core/testing';

import { AMSService } from './ams.service';

describe('AMSService', () => {
  let service: AMSService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AMSService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
