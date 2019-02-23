import { Component } from '@angular/core';
import { EcodeService } from '../../services/ecode.service';
import { Ecode } from '../../models/ecode';

@Component({
    selector: 'ecode-list',
    templateUrl: './ecode-list.component.html'
})

export class EcodeListComponent {
    public ecodes: Ecode[];
    public allEcodes: Ecode[];
    public filter: string;

    constructor(private ecodeService: EcodeService) { }

    ngOnInit() {
        this.ecodeService.getEcodes().subscribe(ecodes => {
            this.ecodes = this.allEcodes = ecodes;
        });
    }

    onFilterCange() {
        var ecodes = this.allEcodes;
        if (this.filter) {
            ecodes = ecodes.filter(c => c.code.indexOf(this.filter) > 0);
            this.ecodes = ecodes;
        } else {
            this.ecodes = this.allEcodes;
        }
    }
}
