import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DestinationSelectorComponent } from './destination-selector.component';

describe('DestinationSelectorComponent', () => {
  let component: DestinationSelectorComponent;
  let fixture: ComponentFixture<DestinationSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DestinationSelectorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DestinationSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
