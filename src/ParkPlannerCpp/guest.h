#ifndef PARKPLANNER_GUEST_H
#define PARKPLANNER_GUEST_H

#include <vector>
#include <iostream>
#include <memory>

namespace challenges {

    namespace park {

        class theme_park;

        class visit;

        class guest {
        private:

            theme_park * _p_theme_park;

            std::vector<int> _desired;

            std::unique_ptr<visit> _sp_best_possible_visit;

        public:

            guest(theme_park * p_theme_park, int entry_time_minutes, std::vector<int> const & desired);

            guest(guest const & other);

            ~guest();

            int _entry_time_minutes;

            void set_desired(std::vector<int> const & desired);

            //visit best_possible_visit();

            bool try_visit();

            static std::vector<guest> read_all(std::istream & is, int count, theme_park * p_theme_park);

            void report(std::ostream & os);

        private:

            static guest read_one(std::istream & is, theme_park * p_theme_park);
        };
    }
}

#endif //PARKPLANNER_GUEST_H
