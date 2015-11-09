#ifndef PARKPLANNER_VISIT_H
#define PARKPLANNER_VISIT_H

#include "guest.h"
#include "theme_park.h"

#include <vector>
#include <memory>

namespace challenges {

    namespace park {

        class visit {
        private:

            guest * _p_guest;

            theme_park * _p_theme_park;

            std::vector<int> _desired;

            static const int _impossible;

            std::unique_ptr<int> _sp_time_minutes;

            int time_minutes();

            int calculate_time();

        public:

            visit(guest * p_guest, theme_park * p_theme_park, std::vector<int> const & desired);

            visit(visit const & other);

            ~visit();

            int time_in_park();

            bool is_impossible();
        };
    }
}

#endif //PARKPLANNER_VISIT_H
