#include "challenge.h"
#include "theme_park.h"

#include <algorithm>

namespace challenges {

    namespace park {

        challenge::challenge(std::istream * pis, std::ostream * pos)
            : challenge_base(pos) {

            read(*pis);
        }

        challenge::~challenge() {
            finish();
        }

        void challenge::read(std::istream & is) {
            _sp_theme_park = theme_park::read(is);
        }

        void challenge::run() {
            auto try_visit = [](guest & g) { g.try_visit(); };
            std::for_each(_sp_theme_park->_guests.begin(), _sp_theme_park->_guests.end(), try_visit);
        }

        void challenge::report(std::ostream & os) {
            auto report_ = [&os](guest & g) { g.report(os); };
            std::for_each(_sp_theme_park->_guests.begin(), _sp_theme_park->_guests.end(), report_);
        }
    }
}